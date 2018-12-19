
#include <windows.h>
#include "DShow.h"
#include <Dshowasf.h>
#include "D3d9.h"
#include "Vmr9.h"
#include "Qedit.h"


#define WM_GRAPHNOTIFY  WM_APP + 1
#define ONE_SECOND 10000000

#pragma comment(lib,"Strmiids.lib")
#pragma comment(lib,"Quartz.lib")
#pragma comment(lib,"wmvcore.lib")


ATOM MyRegisterClass(HINSTANCE hInstance);

HRESULT EnumerateDevices(REFGUID category, IEnumMoniker **ppEnum);

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);

HRESULT WriteBitmap(PCWSTR pszFileName, BITMAPINFOHEADER *pBMI, size_t cbBMI, BYTE *pData, size_t cbData);

BOOL RemoveALLFilters();

extern "C" __declspec(dllexport) bool CaptureImage(BSTR File);

void ErrMsg(LPTSTR szFormat,...);

void HandleGraphEvent();

BOOL StopPreview();

BOOL TearDownGraph();

extern "C" __declspec(dllexport) void ChooseDevice(int index,int flag);

extern "C" __declspec(dllexport) void StopRecordVideo();

extern "C" __declspec(dllexport) bool Clean();
struct _DeviceNames
{
	wchar_t DevNames[20][500];
};

#ifndef SAFE_RELEASE
#define SAFE_RELEASE(x) if (x) { x->Release(); x = NULL; }
#endif

// Macros
#define DibNumColors(lpbi) ((lpbi)->biClrUsed == 0 && (lpbi)->biBitCount <= 8 \
                            ? (int)(1 << (int)(lpbi)->biBitCount)          \
                            : (int)(lpbi)->biClrUsed)

#define DibSize(lpbi) ((lpbi)->biSize + (lpbi)->biSizeImage + (int)(lpbi)->biClrUsed * sizeof(RGBQUAD))

#define BFT_BITMAP 0x4d42

#define DibPaletteSize(lpbi) (DibNumColors(lpbi) * sizeof(RGBQUAD))