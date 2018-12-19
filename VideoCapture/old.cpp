/ Test.cpp : Defines the entry point for the application.
//
#include "stdafx.h"
#include "VideoCapture.h";
#include "md5.h"
#include "aes.h"
#include "CharConverision.h"



int _height;
int _width;
int _MaxVidHeigh;
int _MaxVidWidth;
int _DevIndex;
bool _IsSupportVMR9 = true;
HWND _hParent = NULL;
HWND _hWindow = NULL;
LPCWSTR szWindowClass;


IGraphBuilder *pGraph = NULL;
ICaptureGraphBuilder2 *pBuilder = NULL;
IBaseFilter *pCap = NULL;
IBaseFilter *pMux;
IBaseFilter *pAsfWriter;
IMediaEventEx *pEvent = NULL;
IVideoWindow *pWin = NULL; 
IBaseFilter *pVMR;



void Validate(char* hardware,char* serialNumber)
{
   string str = MD5(hardware).toString();
   str.append("qweqwe943");
   string key =  MD5(str).toString();
   if(key!=serialNumber)
   {
	   ErrMsg(L"序列号错误！");
	   PostQuitMessage(0);
   }
}

void MyFreeMediaType(AM_MEDIA_TYPE& mt)
{
    if (mt.cbFormat != 0)
    {
        CoTaskMemFree((PVOID)mt.pbFormat);
        mt.cbFormat = 0;
        mt.pbFormat = NULL;
    }
    if (mt.pUnk != NULL)
    {
        // Unecessary because pUnk should not be used, but safest.
        mt.pUnk->Release();
        mt.pUnk = NULL;
    }
}

ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= 0;
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1); ;
	wcex.lpszMenuName	= 0;
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= 0;

	return RegisterClassEx(&wcex);
}

HRESULT EnumerateDevices(REFGUID category, IEnumMoniker **ppEnum)
{
    // Create the System Device Enumerator.
    ICreateDevEnum *pDevEnum;
    HRESULT hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL,  
        CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pDevEnum));
    if (SUCCEEDED(hr))
    {
        // Create an enumerator for the category.
        hr = pDevEnum->CreateClassEnumerator(category, ppEnum, 0);
        if (hr == S_FALSE)
        {
            hr = VFW_E_NOT_FOUND;  // The category is empty. Treat as an error.
        }
		SAFE_RELEASE(pDevEnum);
    }
    return hr;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;
	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_ERASEBKGND:
		break;
	case WM_GRAPHNOTIFY:
		HandleGraphEvent();
		break;
	case WM_LBUTTONDOWN:
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

void HandleGraphEvent()
{
    // Disregard if we don't have an IMediaEventEx pointer.
    if (pEvent == NULL)
    {
        return;
    }
    // Get all the events
    long evCode;
    LONG_PTR param1, param2;
    HRESULT hr;
    while (SUCCEEDED(pEvent->GetEvent(&evCode, &param1, &param2, 0)))
    {
        pEvent->FreeEventParams(evCode, param1, param2);
        switch (evCode)
        {
        case EC_COMPLETE:  // Fall through.
			ChooseDevice(_DevIndex);
			break;
        case EC_USERABORT: // Fall through.
			break;
        case EC_ERRORABORT:
            Clean();
            PostQuitMessage(0);
            return;
        }
    } 
}

void DisplayDeviceInformation(IEnumMoniker *pEnum)
{
    IMoniker *pMoniker = NULL;

    while (pEnum->Next(1, &pMoniker, NULL) == S_OK)
    {
        IPropertyBag *pPropBag;
        HRESULT hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
        if (FAILED(hr))
        {
			SAFE_RELEASE(pMoniker);
            continue;  
        } 

        VARIANT var;
        VariantInit(&var);

        // Get description or friendly name.
        hr = pPropBag->Read(L"Description", &var, 0);
        if (FAILED(hr))
        {
            hr = pPropBag->Read(L"FriendlyName", &var, 0);
        }
        if (SUCCEEDED(hr))
        {
            printf("%S\n", var.bstrVal);
            VariantClear(&var); 
        }
		SAFE_RELEASE(pPropBag);
		SAFE_RELEASE(pMoniker);
    }
}

HRESULT EnumFilters (IFilterGraph *pGraph) 
{
    IEnumFilters *pEnum = NULL;
    IBaseFilter *pFilter;
    ULONG cFetched;

    HRESULT hr = pGraph->EnumFilters(&pEnum);
    if (FAILED(hr)) return hr;

    while(pEnum->Next(1, &pFilter, &cFetched) == S_OK)
    {
        FILTER_INFO FilterInfo;
        hr = pFilter->QueryFilterInfo(&FilterInfo);
        if (FAILED(hr))
        {
            MessageBox(NULL, TEXT("Could not get the filter info"),
                TEXT("Error"), MB_OK | MB_ICONERROR);
            continue;  // Maybe the next one will work.
        }


#ifdef UNICODE
        MessageBox(NULL, FilterInfo.achName, TEXT("Filter Name"), MB_OK);
#else
        char szName[MAX_FILTER_NAME];
        int cch = WideCharToMultiByte(CP_ACP, 0, FilterInfo.achName,
            MAX_FILTER_NAME, szName, MAX_FILTER_NAME, 0, 0);
        if (cch > 0)
            MessageBox(NULL, szName, TEXT("Filter Name"), MB_OK);
#endif

        // The FILTER_INFO structure holds a pointer to the Filter Graph
        // Manager, with a reference count that must be released.
        if (FilterInfo.pGraph != NULL)
        {
            FilterInfo.pGraph->Release();
        }
        pFilter->Release();
    }

    pEnum->Release();
    return S_OK;
}

HRESULT AddToRot(IUnknown *pUnkGraph, DWORD *pdwRegister) 
{
    IMoniker * pMoniker;
    IRunningObjectTable *pROT;
    if (FAILED(GetRunningObjectTable(0, &pROT))) {
        return E_FAIL;
    }
    WCHAR wsz[256];
    wsprintfW(wsz, L"FilterGraph %08x pid %08x", (DWORD_PTR)pUnkGraph, GetCurrentProcessId());
    HRESULT hr = CreateItemMoniker(L"!", wsz, &pMoniker);
    if (SUCCEEDED(hr)) {
        hr = pROT->Register(0, pUnkGraph, pMoniker, pdwRegister);
        pMoniker->Release();
    }
    pROT->Release();
    return hr;
} 

void RemoveFromRot(DWORD pdwRegister)
{
    IRunningObjectTable *pROT;
    if (SUCCEEDED(GetRunningObjectTable(0, &pROT))) {
        pROT->Revoke(pdwRegister);
        pROT->Release();
    }
} 

void ErrMsg(LPTSTR szFormat,...)
{
    static TCHAR szBuffer[2048]={0};
    const size_t NUMCHARS = sizeof(szBuffer) / sizeof(szBuffer[0]);
    const int LASTCHAR = NUMCHARS - 1;

    // Format the input string
    va_list pArgs;
    va_start(pArgs, szFormat);

    // Use a bounded buffer size to prevent buffer overruns.  Limit count to
    // character size minus one to allow for a NULL terminating character.
    HRESULT hr = StringCchVPrintf(szBuffer, NUMCHARS - 1, szFormat, pArgs);
    va_end(pArgs);

    // Ensure that the formatted string is NULL-terminated
    szBuffer[LASTCHAR] = TEXT('\0');

    MessageBox(NULL, szBuffer, NULL,
               MB_OK | MB_ICONEXCLAMATION | MB_TASKMODAL);
}

BOOL StartPreview()
{
    IMediaControl *pMC = NULL;
    HRESULT hr = pGraph->QueryInterface(IID_IMediaControl, (void **)&pMC);
    if(SUCCEEDED(hr))
    {
        hr = pMC->Run();
        if(FAILED(hr))
        {
            pMC->Stop();
        }
		SAFE_RELEASE(pMC);
    }
    if(FAILED(hr))
    {
        ErrMsg(TEXT("Error %x: Cannot run preview graph"), hr);
        return FALSE;
    }
    return TRUE;
}

BOOL StopPreview()
{
    IMediaControl *pMC = NULL;
	if(pGraph)
	{
		HRESULT hr = pGraph->QueryInterface(IID_IMediaControl, (void **)&pMC);
		if(SUCCEEDED(hr))
		{
			hr = pMC->Stop();
			SAFE_RELEASE(pMC);
		}
		if(FAILED(hr))
		{
			ErrMsg(TEXT("Error %x: Cannot stop preview graph"), hr);
			return FALSE;
		}
	}
    return TRUE;
}


void WINAPI FreeMediaType(__inout AM_MEDIA_TYPE& mt)
{
    if (mt.cbFormat != 0) {
        CoTaskMemFree((PVOID)mt.pbFormat);

        // Strictly unnecessary but tidier
        mt.cbFormat = 0;
        mt.pbFormat = NULL;
    }
    if (mt.pUnk != NULL) {
        mt.pUnk->Release();
        mt.pUnk = NULL;
    }
}

void WINAPI DeleteMediaType(__inout_opt AM_MEDIA_TYPE *pmt)
{
    // allow NULL pointers for coding simplicity

    if (pmt == NULL) {
        return;
    }

    FreeMediaType(*pmt);
    CoTaskMemFree((PVOID)pmt);
}


void RemoveDownstream(IBaseFilter *pf)
{
    IPin *pP=0, *pTo=0;
    ULONG u;
    IEnumPins *pins = NULL;
    PIN_INFO pininfo;

    if (!pf)
        return;

    HRESULT hr = pf->EnumPins(&pins);
    pins->Reset();

    while(hr == NOERROR)
    {
        hr = pins->Next(1, &pP, &u);
        if(hr == S_OK && pP)
        {
            pP->ConnectedTo(&pTo);
            if(pTo)
            {
                hr = pTo->QueryPinInfo(&pininfo);
                if(hr == NOERROR)
                {
                    if(pininfo.dir == PINDIR_INPUT)
                    {
                        RemoveDownstream(pininfo.pFilter);
                        pGraph->Disconnect(pTo);
                        pGraph->Disconnect(pP);
                        pGraph->RemoveFilter(pininfo.pFilter);
                    }
                    pininfo.pFilter->Release();
                }
                pTo->Release();
            }
            pP->Release();
        }
    }

    if(pins)
        pins->Release();
}

//BOOL StartPlayVideo()
//{
//    IMediaControl *pMC = NULL;
//    HRESULT hr = pGraph2->QueryInterface(IID_IMediaControl, (void **)&pMC);
//	
//    if(SUCCEEDED(hr))
//    {
//        hr = pMC->Run();
//        if(FAILED(hr))
//        {
//            pMC->Stop();
//        }
//        pMC->Release();
//    }
//    if(FAILED(hr))
//    {
//        ErrMsg(TEXT("Error %x: Cannot run preview graph"), hr);
//        return FALSE;
//    }
//    return TRUE;
//}

//BOOL StopPlayVideo()
//{
//    IMediaControl *pMC = NULL;
//    HRESULT hr = pGraph2->QueryInterface(IID_IMediaControl, (void **)&pMC);
//    if(SUCCEEDED(hr))
//    {
//        hr = pMC->Stop();
//        pMC->Release();
//    }
//    if(FAILED(hr))
//    {
//        ErrMsg(TEXT("Error %x: Cannot stop preview graph"), hr);
//        return FALSE;
//    }
//    return TRUE;
//}
BOOL SetFormat()
{
	IAMStreamConfig *pConfig = NULL;
	HRESULT	hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE, 0, pCap, IID_IAMStreamConfig, (void**)&pConfig);
	int iCount = 0, iSize = 0;
	hr = pConfig->GetNumberOfCapabilities(&iCount, &iSize);
	if (iSize == sizeof(VIDEO_STREAM_CONFIG_CAPS))
	{
		if (0 < iCount)
		{
			VIDEO_STREAM_CONFIG_CAPS scc;
			AM_MEDIA_TYPE *pmtConfig;
			VIDEOINFOHEADER *pVih;
			for(int i=0;i<iCount;i++)
			{
				hr = pConfig->GetStreamCaps(i, &pmtConfig, (BYTE*)&scc);
				if(SUCCEEDED(hr))
				{
					if ((pmtConfig->majortype == MEDIATYPE_Video) &&
						(pmtConfig->formattype == FORMAT_VideoInfo) &&
						(pmtConfig->cbFormat >= sizeof (VIDEOINFOHEADER)) &&
						(pmtConfig->pbFormat != NULL))
					{
						pVih = (VIDEOINFOHEADER*)pmtConfig->pbFormat;
						_MaxVidWidth = scc.MaxOutputSize.cx;
						_MaxVidHeigh = scc.MaxOutputSize.cy;
						pVih->bmiHeader.biWidth = _MaxVidWidth;
						pVih->bmiHeader.biHeight = _MaxVidHeigh;
						pVih->bmiHeader.biSizeImage = DIBSIZE(pVih->bmiHeader);
						hr = pConfig->SetFormat(pmtConfig);
					}
					DeleteMediaType(pmtConfig);
				}
			}
		}
	}
	SAFE_RELEASE(pConfig);
	return true;
}


BOOL TearDownGraph()
{
	SAFE_RELEASE(pEvent);
	if(pWin)
    {
		pWin->put_Owner(NULL);
        pWin->put_Visible(OAFALSE);
        pWin->Release();
        pWin = NULL;
    }
	RemoveDownstream(pCap);
	RemoveDownstream(pVMR);
	SAFE_RELEASE(pCap);
	if(pGraph)
	{
		RemoveALLFilters();
	}
	SAFE_RELEASE(pGraph);
	SAFE_RELEASE(pBuilder);
	return true;
}

BOOL RemoveALLFilters()
{
	IEnumFilters *pEnum = NULL;
    IBaseFilter *pFilter;
    ULONG cFetched;
    HRESULT hr = pGraph->EnumFilters(&pEnum);
    if (FAILED(hr)) return hr;
    while(pEnum->Next(1, &pFilter, &cFetched) == S_OK)
    {
        SAFE_RELEASE(pFilter);
    }
    SAFE_RELEASE(pEnum);
	return true;
}

BOOL InitializeWindowlessVMR(HWND hWnd)
{
	
    HRESULT hr = CoCreateInstance( CLSID_VideoMixingRenderer9, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&(pVMR));
	if(FAILED(hr))
	{
		_IsSupportVMR9 = false;
		hr = CoCreateInstance( CLSID_VideoMixingRenderer, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&(pVMR));
	}
	if (SUCCEEDED(hr))
    {
        IVMRFilterConfig* pConfig;
		if(_IsSupportVMR9)
			hr = pVMR->QueryInterface(IID_IVMRFilterConfig9, (void**)&pConfig);
		else
			hr = pVMR->QueryInterface(IID_IVMRFilterConfig, (void**)&pConfig);
        if(SUCCEEDED(hr))
        {
                pConfig->SetRenderingMode(VMRMode_Windowless);
                SAFE_RELEASE(pConfig);
        }
		if(_IsSupportVMR9)
		{
			IVMRWindowlessControl9* pWC = NULL;
			hr = pVMR->QueryInterface(IID_IVMRWindowlessControl9, (void**)&pWC);
			if(SUCCEEDED(hr))
			{
                pWC->SetVideoClippingWindow(_hWindow); //设置视频剪辑窗口
				if(SUCCEEDED(hr))
				{
					long lWidth, lHeight; 
					hr = pWC->GetNativeVideoSize(&lWidth, &lHeight, NULL, NULL); 
					RECT rcSrc, rcDest; 
					SetRect(&rcSrc, 0, 0, lWidth, lHeight); 
					GetClientRect(_hWindow, &rcDest); 
					hr = pWC->SetVideoPosition(NULL, &rcDest); 
					SAFE_RELEASE(pWC);
				}
			}
		}
		else
		{
			IVMRWindowlessControl* pWC = NULL;
			hr = pVMR->QueryInterface(IID_IVMRWindowlessControl, (void**)&pWC);
			if(SUCCEEDED(hr))
			{
                pWC->SetVideoClippingWindow(_hWindow); //设置视频剪辑窗口
				if(SUCCEEDED(hr))
				{
					long lWidth, lHeight; 
					hr = pWC->GetNativeVideoSize(&lWidth, &lHeight, NULL, NULL); 
					RECT rcSrc, rcDest; 
					SetRect(&rcSrc, 0, 0, lWidth, lHeight); 
					GetClientRect(_hWindow, &rcDest); 
					hr = pWC->SetVideoPosition(NULL, &rcDest); 
					SAFE_RELEASE(pWC);
				}
			}
		}
        
    }
    return true;
}

extern "C" __declspec(dllexport) HWND Init(HWND hParent,int width, int height,char* hardware,char* serialNumber)
{
	Validate(hardware,serialNumber);
	_height = height;
	_width = width;
	_hParent = hParent;
	HINSTANCE hInstance = GetModuleHandle(NULL);
	szWindowClass = L"VideoControl";

	MyRegisterClass(hInstance);
	HRESULT hr =  CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
    _hWindow = CreateWindowEx(0,szWindowClass, NULL,
                                    WS_CHILD ,
                                    0, 0,
                                    width, height, 
                                    hParent,
                                    (HMENU)0x001,
                                    NULL,
                                    0);
	
	InitializeWindowlessVMR(_hWindow);
	return _hWindow;
}

extern "C" __declspec(dllexport) BOOL GetDevicesNames(_DeviceNames *pDevNames)
{
	IMoniker *pMoniker = NULL;
	BSTR friendlyName = L"";
	int i = 0;
	IEnumMoniker *pEnum = NULL;
	HRESULT hr = EnumerateDevices(CLSID_VideoInputDeviceCategory, &pEnum);
	if(SUCCEEDED(hr))
	{
		while(pEnum->Next(1, &pMoniker, NULL)==S_OK)
		{
			if(i>=19)break;
			IPropertyBag *pPropBag;
			HRESULT hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
			if(!SUCCEEDED(hr))
			{
				SAFE_RELEASE(pMoniker);
				continue;
			}
			VARIANT var;
			VariantInit(&var);
			hr = pPropBag->Read(L"FriendlyName", &var, 0);
			const size_t NUMCHARS = sizeof(pDevNames->DevNames[i]) / sizeof(pDevNames->DevNames[i][0]);
			const int LASTCHAR = NUMCHARS - 1;
			va_list pArgs;
			va_start(pArgs, var.bstrVal);
		    hr = StringCchVPrintf(pDevNames->DevNames[i], NUMCHARS - 1,  var.bstrVal, pArgs);
			va_end(pArgs);
			VariantClear(&var); 
			SAFE_RELEASE(pMoniker);
			SAFE_RELEASE(pPropBag);
			i++;
		}
	}
	else
	{
		ErrMsg(TEXT("Error %x: 无视频采集卡设备！"), hr);
	}
	SAFE_RELEASE(pEnum);
	return true;
}

extern "C" __declspec(dllexport) void SetSignalSys(int flag)
{
	int signalSys = AnalogVideo_PAL_D;
	switch(flag)
	{
	case 0:
		signalSys = AnalogVideo_PAL_D;
		break;
	case 1:
		signalSys = AnalogVideo_NTSC_M;
		break;
	}
	IAMAnalogVideoDecoder *pAnalogVidDec;
	if(pCap)
	{
		HRESULT hr = pCap->QueryInterface(IID_IAMAnalogVideoDecoder,  (void **)&pAnalogVidDec);
		if(SUCCEEDED(hr))
		{
			hr = pAnalogVidDec->put_TVFormat(signalSys);
			SAFE_RELEASE(pAnalogVidDec);
		}
	}

}

extern "C" __declspec(dllexport) void ChooseDevice(int index)
{
	StopPreview();
	TearDownGraph();
	HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&pGraph);

	hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void **)&pBuilder);
	pBuilder->SetFiltergraph(pGraph);
	_DevIndex = index;
	int i = 0;
	IEnumMoniker *pEnum;
	hr = EnumerateDevices(CLSID_VideoInputDeviceCategory, &pEnum);
	if (SUCCEEDED(hr))
	{


		IMoniker *pMoniker = NULL;
		while (pEnum->Next(1, &pMoniker, NULL) == S_OK)
		{
			if(i==index)
			{

				IPropertyBag *pPropBag;
				hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
				VARIANT var;
				VariantInit(&var);
				// Get description or friendly name.
				hr = pPropBag->Read(L"Description", &var, 0);
				if (FAILED(hr))
				{
					hr = pPropBag->Read(L"FriendlyName", &var, 0);
				}

				hr = pMoniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&pCap);
				if (SUCCEEDED(hr))
				{
					IPropertyBag *pBag;
					WCHAR wachFriendlyName[120];
					hr = pMoniker->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
					hr = StringCchCopyW(wachFriendlyName, sizeof(wachFriendlyName) / sizeof(wachFriendlyName[0]), var.bstrVal);
					SysFreeString(var.bstrVal);
					hr = pGraph->AddFilter(pCap, wachFriendlyName);
					if (FAILED(hr))
					{
						hr = pGraph->AddFilter(pCap, L"Capture Filter");
					}
				}
			}
       		i++;
			SAFE_RELEASE(pMoniker);
		}
		SAFE_RELEASE(pEnum);
	}
	else
	{
		ErrMsg(L"无视频采集设备!");
		return;
	}




	
	hr = pGraph->AddFilter(pVMR, L"Video Mixing Renderer 9");
	if(!SUCCEEDED(hr))
	{
		hr = pBuilder->RenderStream(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCap, NULL, NULL);
	}
	hr = pBuilder->RenderStream(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCap, NULL, pVMR);
	
	if(!SUCCEEDED(hr))
	{
		pGraph->RemoveFilter(pVMR);
		pBuilder->RenderStream(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCap, NULL, NULL);
	}


	pGraph->QueryInterface(IID_IMediaEventEx,(void**)&pEvent); 
	pEvent->SetNotifyWindow((OAHWND)_hWindow, WM_GRAPHNOTIFY, 0);


	/*IVideoWindow *pWin;
	pGraph->QueryInterface(IID_IVideoWindow, (void **)&pWin);
	pWin->SetWindowPosition(0, 0, _width,_height);
	pWin->put_Owner((OAHWND)_hWindow);
	pWin->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS | WS_VISIBLE);
	pWin->put_MessageDrain((OAHWND)_hWindow);*/
	StartPreview();
}

extern "C" __declspec(dllexport) bool Clean()
{
	StopPreview();
	SAFE_RELEASE(pMux);
	SAFE_RELEASE(pAsfWriter);
	SAFE_RELEASE(pMux);
	TearDownGraph();
	SAFE_RELEASE(pVMR);
	CoUninitialize();
	if(pVMR)
	{
		long a =pVMR->Release();
		
	}
	if(_hWindow)
	{
		DestroyWindow(_hWindow);
	}
	return true;
}

extern "C" __declspec(dllexport) void ShowCapPropertyFrame()
{

	ISpecifyPropertyPages *pProp = NULL;
	HRESULT hr = pCap->QueryInterface(IID_ISpecifyPropertyPages,(void**)&pProp);
	if(SUCCEEDED(hr))
	{
		CAUUID caGUID;
		hr = pProp->GetPages(&caGUID);
		IUnknown *pFilterUnk = NULL;
		pFilterUnk = pCap;
		hr = OleCreatePropertyFrame(0, 0, 0, NULL, 1,&pFilterUnk, caGUID.cElems, caGUID.pElems, 0, 0, NULL);
		CoTaskMemFree(caGUID.pElems);
		SAFE_RELEASE(pProp);
	}
	
}

extern "C" __declspec(dllexport) void ShowPreviewPinPropertyFrame()
{
	IAMStreamConfig *pSC;
	HRESULT hr = pBuilder->FindInterface(&PIN_CATEGORY_PREVIEW,
                        &MEDIATYPE_Video, pCap,
                        IID_IAMStreamConfig, (void **)&pSC);
	if(SUCCEEDED(hr))
	{
		ISpecifyPropertyPages *pProp = NULL;
		hr = pSC->QueryInterface(IID_ISpecifyPropertyPages,(void**)&pProp);
		if(SUCCEEDED(hr))
		{
			CAUUID caGUID;
			hr = pProp->GetPages(&caGUID);
			IUnknown *pFilterUnk = NULL;
			pFilterUnk = pCap;
			hr = OleCreatePropertyFrame(0, 0, 0, NULL, 1,&pFilterUnk, caGUID.cElems, caGUID.pElems, 0, 0, NULL);
			CoTaskMemFree(caGUID.pElems);
			SAFE_RELEASE(pProp);
		}
		SAFE_RELEASE(pSC);
	}
	ChooseDevice(_DevIndex);
}

extern "C" __declspec(dllexport) void ShowCapturePinPropertyFrame()
{
	IAMStreamConfig *pSC;
	HRESULT hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
                        &MEDIATYPE_Video, pCap,
                        IID_IAMStreamConfig, (void **)&pSC);
	if(SUCCEEDED(hr))
	{
		ISpecifyPropertyPages *pProp = NULL;
		hr = pSC->QueryInterface(IID_ISpecifyPropertyPages,(void**)&pProp);
		if(SUCCEEDED(hr))
		{
			CAUUID caGUID;
			hr = pProp->GetPages(&caGUID);
			IUnknown *pFilterUnk = NULL;
			pFilterUnk = pCap;
			hr = OleCreatePropertyFrame(0, 0, 0, NULL, 1,&pFilterUnk, caGUID.cElems, caGUID.pElems, 0, 0, NULL);
			CoTaskMemFree(caGUID.pElems);
			SAFE_RELEASE(pProp);
		}
		SAFE_RELEASE(pSC);
	}
	ChooseDevice(_DevIndex);
}

extern "C" __declspec(dllexport) bool GetVMRPRocAmpControl(VMR9ProcAmpControl *lpControl)
{
	IVMRMixerControl9 *Config9 = NULL;
	HRESULT hr = pVMR->QueryInterface( IID_IVMRMixerControl9, (void**)&Config9);//成功
	if(SUCCEEDED(hr))
	{
		//VMR9ProcAmpControl control ;
		memset(lpControl, 0, sizeof(VMR9ProcAmpControl));
		lpControl->dwSize = sizeof(VMR9ProcAmpControl);
		Config9->GetProcAmpControl(0,lpControl);
		SAFE_RELEASE(Config9);
		return true;
	}
	return false;
}


extern "C" __declspec(dllexport) bool GetVMRBrightnessRange(VMR9ProcAmpControlRange *lpControlRange)
{
	IVMRMixerControl9 *Config9 = NULL;
	HRESULT hr = pVMR->QueryInterface( IID_IVMRMixerControl9, (void**)&Config9);//成功
	if(SUCCEEDED(hr))
	{
		memset(lpControlRange, 0, sizeof(VMR9ProcAmpControlRange));
		lpControlRange->dwSize = sizeof(VMR9ProcAmpControlRange);
		lpControlRange->dwProperty = ProcAmpControl9_Brightness;
		hr = Config9->GetProcAmpControlRange(0,lpControlRange);
		return true;
	}
	return false;
}

extern "C" __declspec(dllexport) bool GetVMRContrastRange(VMR9ProcAmpControlRange *lpControlRange)
{
	IVMRMixerControl9 *Config9 = NULL;
	HRESULT hr = pVMR->QueryInterface( IID_IVMRMixerControl9, (void**)&Config9);//成功
	if(SUCCEEDED(hr))
	{
		memset(lpControlRange, 0, sizeof(VMR9ProcAmpControlRange));
		lpControlRange->dwSize = sizeof(VMR9ProcAmpControlRange);
		lpControlRange->dwProperty = ProcAmpControl9_Contrast;
		hr = Config9->GetProcAmpControlRange(0,lpControlRange);
		return true;
	}
	return false;
}

extern "C" __declspec(dllexport) bool GetVMRHueRange(VMR9ProcAmpControlRange *lpControlRange)
{
	IVMRMixerControl9 *Config9 = NULL;
	HRESULT hr = pVMR->QueryInterface( IID_IVMRMixerControl9, (void**)&Config9);//成功
	if(SUCCEEDED(hr))
	{
		memset(lpControlRange, 0, sizeof(VMR9ProcAmpControlRange));
		lpControlRange->dwSize = sizeof(VMR9ProcAmpControlRange);
		lpControlRange->dwProperty = ProcAmpControl9_Hue;
		hr = Config9->GetProcAmpControlRange(0,lpControlRange);
		return true;
	}
	return false;
}

extern "C" __declspec(dllexport) bool GetVMRSaturationRange(VMR9ProcAmpControlRange *lpControlRange)
{
	IVMRMixerControl9 *Config9 = NULL;
	HRESULT hr = pVMR->QueryInterface( IID_IVMRMixerControl9, (void**)&Config9);//成功
	if(SUCCEEDED(hr))
	{
		memset(lpControlRange, 0, sizeof(VMR9ProcAmpControlRange));
		lpControlRange->dwSize = sizeof(VMR9ProcAmpControlRange);
		lpControlRange->dwProperty = ProcAmpControl9_Saturation;
		hr = Config9->GetProcAmpControlRange(0,lpControlRange);
		return true;
	}
	return false;
}

extern "C" __declspec(dllexport) bool SetVMRPRocAmpControl(float value, int flag)
{
	IVMRMixerControl9 *Config9 = NULL;
	HRESULT hr = pVMR->QueryInterface( IID_IVMRMixerControl9, (void**)&Config9);//成功
	if(SUCCEEDED(hr))
	{
		VMR9ProcAmpControl *lpControl = new VMR9ProcAmpControl;
		memset(lpControl, 0, sizeof(VMR9ProcAmpControl));
		lpControl->dwSize = sizeof(VMR9ProcAmpControl);
		Config9->GetProcAmpControl(0,lpControl);
		switch(flag)
		{
		case 1:
			lpControl->Brightness = value;
			break;
		case 2:
			lpControl->Contrast = value;
			break;
		case 3:
			lpControl->Hue = value;
			break;
		case 4:
			lpControl->Saturation = value;
			break;
		}
		HRESULT hr = Config9->SetProcAmpControl(0,lpControl);
		delete lpControl;
		SAFE_RELEASE(Config9);
		return true;
	}
	return false;
}

extern "C" __declspec(dllexport) bool PlayVideo(BSTR FilePath)
{
	StopPreview();
	TearDownGraph();
	HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&pGraph);
	if(SUCCEEDED(hr))
	{
		hr = pGraph->AddFilter( pVMR, L"Video Mixing Renderer 9");
		hr = pGraph->RenderFile(FilePath, NULL);
		pGraph->QueryInterface(IID_IMediaEventEx,(void**)&pEvent); 
		pEvent->SetNotifyWindow((OAHWND)_hWindow, WM_GRAPHNOTIFY, 0);
		/*pGraph->QueryInterface(IID_IVideoWindow, (void **)&pWin);
		pWin->SetWindowPosition(-5, 0, _width,_height);
		pWin->put_Owner((OAHWND)_hWindow);
		pWin->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS);*/
		StartPreview();
	}
	return 0;
}

extern "C" __declspec(dllexport) bool SetVideoPostion(REFERENCE_TIME *pNow,REFERENCE_TIME  *pStop)
{
	 IMediaSeeking *pSeek = NULL;
	 if(pGraph)
	 {
		 HRESULT hr = pGraph->QueryInterface(IID_IMediaSeeking, (void **)&pSeek);
		 if(SUCCEEDED(hr))
		 {
			 DWORD dwCap = 0;
			 hr = pSeek->GetCapabilities(&dwCap);
			 if (AM_SEEKING_CanSeekAbsolute & dwCap)
			 {
				pSeek->SetPositions(pNow,AM_SEEKING_AbsolutePositioning,pStop,AM_SEEKING_AbsolutePositioning);
			 }
			 SAFE_RELEASE(pSeek);
		 }
	 }
	 return true;
}

extern "C" __declspec(dllexport) bool GetVideoPostion(REFERENCE_TIME *pNow,REFERENCE_TIME  *pStop)
{
	 IMediaSeeking *pSeek = NULL;
	 if(pGraph)
	 {
		 HRESULT hr = pGraph->QueryInterface(IID_IMediaSeeking, (void **)&pSeek);
		 if(SUCCEEDED(hr))
		 {
			 DWORD dwCap = 0;
			 hr = pSeek->GetCapabilities(&dwCap);
			 if (AM_SEEKING_CanSeekAbsolute & dwCap)
			 {
				pSeek->GetPositions(pNow,pStop);
			 }
			 SAFE_RELEASE(pSeek);
		 }
	 }
	 return true;
}

extern "C" __declspec(dllexport) bool GetDuration(REFERENCE_TIME *pDuration)
{
	 IMediaSeeking *pSeek = NULL;
	 if(pGraph)
	 {
		 HRESULT hr = pGraph->QueryInterface(IID_IMediaSeeking, (void **)&pSeek);
		 if(SUCCEEDED(hr))
		 {
			 DWORD dwCap = 0;
			 hr = pSeek->GetCapabilities(&dwCap);
			 if (AM_SEEKING_CanSeekAbsolute & dwCap)
			 {
				pSeek->GetDuration(pDuration);
			 }
			 SAFE_RELEASE(pSeek);
		 }
	 }
	 return true;
}

extern "C" __declspec(dllexport) bool CaptureImage(BSTR File)
{
	IVMRWindowlessControl9 *pWC9 = NULL;
	IVMRWindowlessControl *pWC = NULL;
	HRESULT hr = 0;
	if(_IsSupportVMR9)
	{
		hr = pVMR->QueryInterface(IID_IVMRWindowlessControl9, (void**)&pWC9);//成功
	}
	else
	{
		hr = pVMR->QueryInterface(IID_IVMRWindowlessControl, (void**)&pWC);//成功
	}
	if(SUCCEEDED(hr))
	{
		BYTE* lpCurrImage = NULL;
		if(_IsSupportVMR9)
			hr = pWC9->GetCurrentImage(&lpCurrImage);
		else
			hr = pWC->GetCurrentImage(&lpCurrImage);
		if(SUCCEEDED(hr))
		{
			BITMAPFILEHEADER hdr;
			DWORD dwSize, dwWritten;
			LPBITMAPINFOHEADER  pdib = (LPBITMAPINFOHEADER) lpCurrImage;
			HANDLE hFile = CreateFile(File, GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
			if (hFile == INVALID_HANDLE_VALUE)
                return FALSE;


			dwSize = DibSize(pdib);
			hdr.bfType = BFT_BITMAP;
            hdr.bfSize = dwSize + sizeof(BITMAPFILEHEADER);
            hdr.bfReserved1 = 0;
            hdr.bfReserved2 = 0;
            hdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) + pdib->biSize + DibPaletteSize(pdib);

			WriteFile(hFile, (LPCVOID) &hdr, sizeof(BITMAPFILEHEADER), &dwWritten, 0);
            WriteFile(hFile, (LPCVOID) pdib, dwSize, &dwWritten, 0);
			
			   
			CloseHandle(hFile);

            // The app must free the image data returned from GetCurrentImage()
            CoTaskMemFree(lpCurrImage);
		}
		SAFE_RELEASE(pWC);
	}
	return true;
}

extern "C" __declspec(dllexport) bool RecordVideo(BSTR File)
{
	StopPreview();
	TearDownGraph();
	HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&pGraph);
	hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void **)&pBuilder);
	pBuilder->SetFiltergraph(pGraph);
	
	int i = 0;
	IEnumMoniker *pEnum;
	 hr = EnumerateDevices(CLSID_VideoInputDeviceCategory, &pEnum);
	if (SUCCEEDED(hr))
	{
		IMoniker *pMoniker = NULL;
		while (pEnum->Next(1, &pMoniker, NULL) == S_OK)
		{
			if(i==_DevIndex)
			{
				IPropertyBag *pPropBag;
				hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
				VARIANT var;
				VariantInit(&var);
				// Get description or friendly name.
				hr = pPropBag->Read(L"Description", &var, 0);
				if (FAILED(hr))
				{
					hr = pPropBag->Read(L"FriendlyName", &var, 0);
				}

				hr = pMoniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&pCap);
				if (SUCCEEDED(hr))
				{
					IPropertyBag *pBag;
					WCHAR wachFriendlyName[120];
					hr = pMoniker->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
					hr = StringCchCopyW(wachFriendlyName, sizeof(wachFriendlyName) / sizeof(wachFriendlyName[0]), var.bstrVal);
					SysFreeString(var.bstrVal);
					hr = pGraph->AddFilter(pCap, wachFriendlyName);
					if (FAILED(hr))
					{
						hr = pGraph->AddFilter(pCap, L"Capture Filter");
					}
				}
			}
       		i++;
			SAFE_RELEASE(pMoniker);
		}
		SAFE_RELEASE(pEnum);
	}
	else
	{
		ErrMsg(L"无视频采集设备!");
		return false;
	}
	hr = pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW, &MEDIATYPE_Video, pCap, NULL, NULL);
	SetFormat();

	
    hr = pBuilder->SetOutputFileName(&MEDIASUBTYPE_Avi, File, &pMux, NULL);
	hr = pBuilder->RenderStream(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCap, NULL, pMux);
	pGraph->QueryInterface(IID_IMediaEventEx,(void**)&pEvent); 
	pEvent->SetNotifyWindow((OAHWND)_hWindow, WM_GRAPHNOTIFY, 0);
	
	pGraph->QueryInterface(IID_IVideoWindow, (void **)&pWin);
	pWin->SetWindowPosition(0, 0, _width,_height);
	pWin->put_Owner((OAHWND)_hWindow);
	pWin->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS | WS_VISIBLE);
	pWin->put_MessageDrain((OAHWND)_hWindow);
	StartPreview();
}

extern "C" __declspec(dllexport) bool RecordWMVVideo(BSTR file, BSTR profile, long bitrate)
{
	StopPreview();
	TearDownGraph();
	HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&pGraph);

	hr = CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, (void **)&pBuilder);

	hr = pBuilder->SetFiltergraph(pGraph);



	int i = 0;
	IEnumMoniker *pEnum;
	 hr = EnumerateDevices(CLSID_VideoInputDeviceCategory, &pEnum);
	if (SUCCEEDED(hr))
	{
		IMoniker *pMoniker = NULL;
		while (pEnum->Next(1, &pMoniker, NULL) == S_OK)
		{
			if(i==_DevIndex)
			{
				IPropertyBag *pPropBag;
				hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
				VARIANT var;
				VariantInit(&var);
				// Get description or friendly name.
				hr = pPropBag->Read(L"Description", &var, 0);
				if (FAILED(hr))
				{
					hr = pPropBag->Read(L"FriendlyName", &var, 0);
				}

				hr = pMoniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&pCap);
				if (SUCCEEDED(hr))
				{
					IPropertyBag *pBag;
					WCHAR wachFriendlyName[120];
					hr = pMoniker->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
					hr = StringCchCopyW(wachFriendlyName, sizeof(wachFriendlyName) / sizeof(wachFriendlyName[0]), var.bstrVal);
					SysFreeString(var.bstrVal);
					hr = pGraph->AddFilter(pCap, wachFriendlyName);
					if (FAILED(hr))
					{
						hr = pGraph->AddFilter(pCap, L"Capture Filter");
						
					}
				
				}
			}
       		i++;
			SAFE_RELEASE(pMoniker);
		}
		SAFE_RELEASE(pEnum);
	}
	else
	{
		ErrMsg(L"无视频采集设备!");
		return false;
	}
	
	hr = pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW, &MEDIATYPE_Video, pCap, NULL, NULL);
		
	SetFormat();
    hr = pBuilder->SetOutputFileName(&MEDIASUBTYPE_Asf, file, &pAsfWriter, NULL); 
	IConfigAsfWriter *pConfig = 0;
	hr = pAsfWriter->QueryInterface(IID_IConfigAsfWriter, (void**)&pConfig);
	if (SUCCEEDED(hr))
	{
		IWMProfileManager *pProfileMgr = NULL;
		hr = WMCreateProfileManager(&pProfileMgr);	
		if(SUCCEEDED(hr))
		{
			IWMProfile *pProfile;
			hr = pProfileMgr->LoadProfileByData(profile,&pProfile);
			
			if(SUCCEEDED(hr))
			{
				/*pProfileMgr->CreateEmptyProfile(WMT_VER_8_0,&pProfile);*/
				IWMStreamConfig *pStreamCfg;
				//pProfile->CreateNewStream(WMMEDIATYPE_Video,&pStreamCfg);
				//pProfile->AddStream(pStreamCfg);
				pProfile->GetStreamByNumber(1,&pStreamCfg);
				hr = pStreamCfg->SetBitrate(bitrate);
				IWMMediaProps *pMediaProps;
				WM_MEDIA_TYPE *mediaType; 
				hr = pStreamCfg->QueryInterface(IID_IWMMediaProps ,  (void**) &pMediaProps);
		
				DWORD bufsize;  
				pMediaProps->GetMediaType(NULL,&bufsize);  
				BYTE *pBuf = new BYTE[bufsize];
				mediaType = (WM_MEDIA_TYPE*) pBuf;  
				pMediaProps->GetMediaType(mediaType,&bufsize);


				VIDEOINFOHEADER *pVih = reinterpret_cast<VIDEOINFOHEADER*>(mediaType->pbFormat);
				pVih->dwBitRate = bitrate;
				pVih->bmiHeader.biWidth = _MaxVidWidth;
				pVih->bmiHeader.biHeight = _MaxVidHeigh;
				pVih->bmiHeader.biSizeImage = DIBSIZE(pVih->bmiHeader);
				mediaType->lSampleSize = pVih->bmiHeader.biSizeImage;
				RECT src = {0,0,pVih->bmiHeader.biWidth,pVih->bmiHeader.biHeight};  
				pVih->rcSource = src;  
				pVih->rcTarget = src;  

				hr = pMediaProps->SetMediaType (mediaType); 
				hr = pProfile->ReconfigStream (pStreamCfg);  
				hr = pConfig->ConfigureFilterUsingProfile(pProfile);
				SAFE_RELEASE(pStreamCfg);
				SAFE_RELEASE(pMediaProps);
				SAFE_RELEASE(pProfile);

				delete []pBuf;
			}
			SAFE_RELEASE(pProfileMgr);
		}
		SAFE_RELEASE(pConfig);
	}
	hr = pBuilder->RenderStream(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCap, NULL, pAsfWriter);
	pGraph->QueryInterface(IID_IMediaEventEx,(void**)&pEvent); 
	pEvent->SetNotifyWindow((OAHWND)_hWindow, WM_GRAPHNOTIFY, 0);
	pGraph->QueryInterface(IID_IVideoWindow, (void **)&pWin);
	pWin->SetWindowPosition(0, 0, _width,_height);
	pWin->put_Owner((OAHWND)_hWindow);
	pWin->put_WindowStyle(WS_CHILD | WS_CLIPSIBLINGS | WS_VISIBLE);
	pWin->put_MessageDrain((OAHWND)_hWindow);
	StartPreview();
}

extern "C" __declspec(dllexport) void StopRecordVideo()
{
	StopPreview();
	SAFE_RELEASE(pMux);
	SAFE_RELEASE(pAsfWriter);
	SAFE_RELEASE(pWin);
	if(pWin)
	{
	    pWin->put_Owner(NULL);
        pWin->put_Visible(OAFALSE);
        pWin->Release();
        pWin = NULL;
	}
	ChooseDevice(_DevIndex);
}

extern "C" __declspec(dllexport) void GetVideoSource()
{
	IAMCrossbar *pX, *pX2;
    IBaseFilter *pXF;
    // we could use better error checking here... I'm assuming
    // this won't fail
    HRESULT hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCap, IID_IAMCrossbar, (void **)&pX);
	if (SUCCEEDED(hr)) 
    {
		
		hr = pX->QueryInterface(IID_IBaseFilter, (void **)&pXF);
		if (SUCCEEDED(hr)) 
		{
			 ISpecifyPropertyPages *pSpec;
                CAUUID cauuid;
			  hr = pX->QueryInterface(IID_ISpecifyPropertyPages,
                    (void **)&pSpec);
                if(hr == S_OK)
                {
                    hr = pSpec->GetPages(&cauuid);

                    hr = OleCreatePropertyFrame(_hWindow, 30, 30, NULL, 1,
                        (IUnknown **)&pX, cauuid.cElems,
                        (GUID *)cauuid.pElems, 0, 0, NULL);

                    CoTaskMemFree(cauuid.pElems);
                    pSpec->Release();
                }
                pX->Release();
		}
		SAFE_RELEASE(pX);
	}

}

extern "C"  __declspec(dllexport) void ClipVideo(int top, int left, int right, int bottom, bool fullWindow)
{
	IVMRWindowlessControl9* pWC = NULL;
    HRESULT hr = pVMR->QueryInterface(IID_IVMRWindowlessControl9, (void**)&pWC);
    if(SUCCEEDED(hr))
    {
		if(SUCCEEDED(hr))
		{
			RECT rcSrc, rcDest; 
			SetRect(&rcSrc, top, left, right, bottom); 
			GetClientRect(_hWindow, &rcDest); 
			if(fullWindow)
			{
				ChooseDevice(_DevIndex);
			}
			else
			{
				hr = pWC->SetVideoPosition(&rcSrc, &rcDest); 
			}
			SAFE_RELEASE(pWC);
		}
    }
}

extern "C"  __declspec(dllexport) bool  Pause()
{
    IMediaControl *pMC = NULL;
	if(pGraph)
	{
		HRESULT hr = pGraph->QueryInterface(IID_IMediaControl, (void **)&pMC);
		if(SUCCEEDED(hr))
		{
			hr = pMC->Pause();
			SAFE_RELEASE(pMC);
			return true;
		}
		if(FAILED(hr))
		{
			ErrMsg(TEXT("Error %x: Cannot Pause preview graph"), hr);
			return false;
		}
	}
}

extern "C"  __declspec(dllexport) void StartPlayVideo()
{
    StartPreview();
}

extern "C"  __declspec(dllexport) void Stop()
{
    StopPreview();
}

extern "C"  __declspec(dllexport) void GenerateKey(char* number,char* key)
{
	 string str = MD5(number).toString();
	 strcpy(key,str.c_str());
}