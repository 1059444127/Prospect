   M S B A M L   `   `   `  ����C  ?Prospect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:com.cloud.prospectcom.cloud.prospect  5          com.cloud.prospect.UCMain   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:com.cloud.prospect     5      X TXceed.Wpf.Toolkit, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5        $��13(�5   4   .��5
      � -   5          �Name$  grdMain��5
       � ColumnDefinitions 5   
   �� 5      $��1 �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��1��6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      �� -   5         ��Name$ 
gbxPatient��5   ~   $��病人信息��6   $
��Left=�6%   $	��144��6@   $	��Top=�6M   $	��750��6e   $��0��6q   ��5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ Black��6'    � Offset$ 0Q�65   � 5      $ 	#FF330B0B��6'   $ 1Q�69   5      .��5      � $	��750��6   .+�5       �� $��姓名��6   $
��Left=�6)   $��0,1.686,0,0q�6D   $	��Top=�6Z   $��55.457��6s   �� 5!      $��	住院号��6   $
��Left=�6*   $��0.513,29.843,0,0q�6E   $	��Top=�6_   $��55.457��6w    J� RenderTransformOrigin$ 0.679,-0.006&�6�   �� 5"      $��检查部位��6   $
��Left=�6,   $��0,56.999,0,0q�6G   $��64.457��6^   $	��Top=�6m   �� 5#      -   5        	 ��Name$	 tbxName��5#      
 �� 	MaxLength$
 30��60   $
��Left=�6U   $��68.457,4,0,0q�6p   $	��Top=�6�   $��87.154��6�    �� MaxLines$ 1��6�   ��6?   �� *Name+6�   �� 5$      $
��Left=�6D   $��68.97,31.156,0,0q�6_   $	��Top=�6y   $��87.154��6�   $ 1��6�   ��6   �� *Hospitalized_Number+6�   �� 5%      $
 5��6>   $��68.97,58.312,0,0q�6L   $	��Top=�6f   $
��Left=�6~   $��197.487��6�   $ 1��6�   ��6   �� *Examine_Part+6�   �� 5&      -   5        $��cbxDepartment��5&       �� 
IsEditable . $
��Left=�6i   $��68.97,85.468,0,0q�6�   $��86.641��6�   $	��Top=�6�    �� Text 6F   �� *Apply_Department+6�   �� 5'      $��申请科室��6   $
��Left=�6,   $��0.513,84.155,0,0q�6G   $ -0.939,-2.051&�6a   $��64.457��6�   $	��Top=�6�   �� 5(      $��性别��6   $
��Left=�6)   $��155.611,2.676,0,0q�6D   $	��Top=�6_   $��44.056��6w   $ -0.419,0.587&�6�   �� 5)       .  �� 
IsReadOnly . $��205.124,4,0,0q�6X   $	��Top=�6o   $
��Left=�6�   $��61.333��6�   $ 0.457,0.828&�6�    6   �� *Sex+.@�5*      �� $	��男��6'   �� 5+      $	��女��6'   5,      �� 5-      $��	超声号��6   $
��Left=�6*   $��266.457,1.686,0,0q�6E   $	��Top=�6`   $��60.457��6x   �� 5.      $��333.457,4,228.142,0q�6B   $	��Top=�6_   ���	#FFCDF3EB$ 1��6�    �� 
IsReadOnly . ��6   �� *Ultrasound_Number+6�   �� 5/      $��	门诊号��6   $
��Left=�6+   $��521.858,1.686,0,0q�6F   $	��Top=�6b   $��63.371��6z   $ 3.078,0.587&�6�   �� 50      $
��Left=�6=   $��600.872,4,0,0q�6X   $	��Top=�6o   $��140.599��6�   $ 1��6�   ��6   �� *Clinic_Number+6�   �� 51      $��床号��6   $
��Left=�6)   $��156.124,29.833,0,0q�6D   $	��Top=�6a   $��44.056��6y   $ 0.686,0.552&�6�   �� 52      $
��Left=�6>   $��205.124,31.156,0,0q�6Y   $	��Top=�6u   $��61.333��6�   $ 1��6�   ��6   �� *Sickbed_Number+6�   �� 53      $��检查时间��6   $��266.457,29.843,0,0q�6+   $
��Left=�6H   $	��Top=�6c   $��67��6|   �� 54      $��年龄��6   $
��Left=�6)   $��521.858,29.843,0,0q�6D   $	��Top=�6`   $��38.056��6x   �� 55      $
��Left=�63   $��600.872,29.843,0,0q�6N   $	��Top=�6j   $��69.432��6�   $ 1��6�   $
 3��6�   ��6   �� *Age+6�   �� 56       .  . $��673.775,29.833,0,0q�6^   $
��Left=�6z   $	��Top=�6�    �� SelectedIndex$ 0��6�   $��67.696��6�    6    �� *	AgeType+.@�57      �� $	��岁��6'   �� 58      $	��月��6'   �� 59      $	��周��6'   �� 5:      $	��天��6'   5;      &   Xceed.Wpf.Toolkit.DateTimePicker  5<      $��333.457,31.156,228.142,0q�6I   $	��Top=�6k      
TimeFormat$ Custom=�6�      TimeFormatString$ yyyy-MM-dd HH:mm:ss��6�      TimeWatermark$  ��6�   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 6*   �� *Examine_Time+6�   �� 5=      $��记录医师��6   $��266.457,56.312,0,0q�6+   $	��Top=�6G   $
��Left=�6_   $��67��6z   �� 5>      -   5        $��cbxRecordDoctor��5>      $��333.457,56.312,0,0q�6V   $	��Top=�6r   $
��Left=�6�   $��188.401��6�    .  66   �� *Record_Doctor+5?      �� 5@      $��诊断医师��6   $��521.858,58.312,0,0q�6+   $ -0.939,-2.051&�6G   $	��Top=�6m   $
��Left=�6�   $��67.457��6�   �� 5A      -   5        $��cbxDiagnosisDoctor��5A      $��0,56.312,8.529,0q�6[   $	��Top=�6u   $��Right=�6�   $��140.599��6�    .  69   �� *Diagnosis_Doctor+5B      �� 5C      $��申请医师��6   $
��Left=�6+   $��155.611,84.155,0,0q�6F   $��64.457��6b   $	��Top=�6q   �� 5D      -   5        $��cbxApplyDoctor��5D      $��220.068,85.468,0,0q�6S   $	��Top=�6o   $
��Left=�6�    . $	��126��6�    65   �� *Apply_Doctor+5E      �� 5F      $��临床诊断��6   $
��Left=�6+   $��346.068,82.781,0,0q�6F   $��64.457��6b   $	��Top=�6q   �� 5G      -	   5        $��cbxDiagnosis��5G      $��0,83.468,8.529,0q�6W   $	��Top=�6q    . $��Right=�6�   $��309.513��6�    63   �� *Clinical_Diagnosis+5H      5I      5J      � 5K      $��1��6    5L      �� 5M      $
��Auto �6$   �� 5N      $
��0.5* �6$   �� 5O      $
��Auto �6$   �� 5P      $
��0.5* �6$    5R      �� 5S      $	��560 �6'   �� 5T      $��* �6'   �� 5U      $	��150 �6'   .+�5X      � $��30��6   $��0��6#   $��0��61   .+�5Y      �� $��1q�6   
�������
�������$
��Left=�6\   $��0,8,0,-1q�6x   $��70��6�   ._�5Z      �� $
��Left=�6$   $
��Wrap=�6?   $��16(�6S   $��69��6a   ���	#FF465DA3��5[      �� 5\      $	 1,0&�60   $	 0,0&�6?   .��5]      � $ 	#FFF9F9F9��6*   $ 0.073Q�6<   � 5^      $ 	#FFB5E0C9��6*   $ 0.839Q�6<   5_      �� 5`   2    � Language$ zh-cn�66   $�超声所见��6G   6V   5a      �� 5b      -
   5         ��Name$ 
cbxPostive��5b      $��阳性��63   $��435,13.233,0,0.766q�6@   $
��Left=�6w   6�   �� 5c      -   5        $ cbxNegative��5c      $��阴性��64   $��
0,14,35,-1q�6A   $��Right=�6U   $��45��6�   �� 5d      -   5         ��Name$ 
btnFinding��5d   �   $��Right=�6   $��0,12,0,0q�6:   $��29��6L   $ 
0.5,-0.263&�6W   $��10.667(�6z    � ToolTip$ 超声所见简洁模板��6�   ��5e      �� 5f      $	 0,1&�62   $	 0,0&�6A   .��5g   "   � $ 	#FFDDEBE5��6/   $ 0.008Q�6A   � 5h   "   $ 	#FFADF9DD��6/   $ 0.488Q�6A   � 5i   "   $ 	#FF8FF5CF��6/   $ 1Q�6A   5j      ��5l      � 5m      .+�5n   "   �� $��20��6)   $��20��65   $��0,-4.75,0,-3.25q�6@   $.��(/Prospect;component/img/opentemplate.png��6Y   5o      5r      5s      � 5t      $��1��6   $��0��6$   .+�5u      �� -   5        $	 
tbxFinding��5u      $��0��6W    �� TextWrapping$
 Wrap=�6�   $��15(�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   $��1,0,0,0q�6�   ��63   �� *Ultrasound_Finding+6�   5v      � 5w      $��30��6   $��2��6#   $��0��60   .+�5x      �� $��1q�6   
�������
�������$��
0,8,489,-1q�6\   ._�5y      �� $
��Left=�6$   $
��Wrap=�6?   $��16(�6S   $��69��6a   ���	#FF465DA3��5z      �� 5{      $	 1,0&�60   $	 0,0&�6?   .��5|      � $ 	#FFF9F9F9��6*   $ 0.073Q�6<   � 5}      $ 	#FFB5E0C9��6*   $ 0.839Q�6<   5~      �� 5   2   $ zh-cn�66   $�超声提示��6G   6W   5�      �� 5�      -   5        $ 	btnPrompt��5�   �   $��Right=�6   $��0,12,0,0q�6:   $��29��6L   $ 
0.5,-0.263&�6W   $��10.667(�6z   $ 超声提示简洁模板��6�   ��5�      �� 5�      $	 0,1&�62   $	 0,0&�6A   .��5�   "   � $ 	#FFDDEBE5��6/   $ 0.008Q�6A   � 5�   "   $ 	#FFADF9DD��6/   $ 0.488Q�6A   � 5�   "   $ 	#FF8FF5CF��6/   $ 1Q�6A   5�      ��5�      � 5�      .+�5�   "   �� $��20��6)   $��20��65   $��0,-4.75,0,-3.25q�6@   $.��(/Prospect;component/img/opentemplate.png��6Y   5�      5�      5�      � 5�      $��3��6   $��0��6$   .+�5�      �� -   5        $��	tbxPrompt��5�   A   $	��590��6R   $
 Wrap=�6^   $��15(�6r   $
 Auto=�6�   $��1,0,0,6q�6�   ��6   �� *Ultrasound_Prompt+6�   5�      �� 5�       �� Orientation$ Vertical=�6   $��0��64   $��1��6A   $��4��6Q   .+�5�      �� -   5        $��btnChar1��5�   +   $
��Left=�5�      $��Stretch=�5�         ToolButtonStyle#��[  5�      $	��～��67   $��15(�6C   $��25��6Q   $��40��6]   5�      �� 5�      -   5        $��btnChar2��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��%��67   $��15(�6C   $��25��6Q   $��40��6]   6j   �� 5�      -   5        $��btnChar3��5�   +   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��X��67   $��15(�6C   $��25��6Q   $��40��6]   6j   �� 5�      -   5        $��btnChar4��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��cm��67   $��15(�6D   $��25��6R   $��40��6^   5�      �� 5�      -   5        $��btnChar5��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��mm��67   $��15(�6D   $��25��6R   $��40��6^   5�      �� 5�      -   5        $��btnChar6��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $
��cm/s��67   $��14(�6F   $��25��6T   $��40��6`   5�      �� 5�      -   5        $��btnChar7��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��RI��67   $��15(�6D   $��25��6R   $��40��6^   5�      �� 5�      -   5        $��btnChar8��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��PI��67   $��15(�6D   $��25��6R   $��40��6^   5�      �� 5�      -   5        $��btnChar9��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $
��Vmax��67   $��12(�6F   $��25��6T   $��40��6`   5�      �� 5�      -   5        $��	btnChar10��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $
��Vmin��67   $��12(�6F   $��25��6T   $��40��6`   5�      �� 5�      -   5        $��	btnChar11��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $	��℃��67   $��15(�6C   $��25��6Q   $��40��6]   5�      �� 5�      -   5        $��	btnChar12��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��α��67   $��15(�6C   $��25��6Q   $��40��6]   5�      �� 5�      -   5        $��	btnChar13��5�   ,   $
��Left=�5�      $��Stretch=�5�      #��[  5�      $��β��67   $��15(�6C   $��25��6Q   $��40��6]   5�      5�      � 5�      $��2��6   $��0��6'   .+�5�      �� $��1q�6   
�������$
��Left=�6H   $��1,8,0,0q�6c   $��69��6t   ._�5�      �� $
��Wrap=�6$   $��16(�68   ���	#FF465DA3��5�      �� 5�      $	 1,0&�60   $	 0,0&�6?   .��5�      � $ 	#FFF9F9F9��6*   $ 0.073Q�6<   � 5�      $ 	#FFB5E0C9��6*   $ 0.839Q�6<   5�      �� 5�   2   $ zh-cn�66   $�快捷模板��6G   6V   5�      5�      � 5�      $��2��6   $��1��6'   $��3��64   .+�5�      R� -   5         R�Name$ trvTemplate��5�      $��1,-1,1,6q�64   ��5�      �� 5�       �� ItemsSource 67   �� *
Children+.R�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   ��  ��Name$ imgTreeItem��6,   $��10��6A   $��10��6M   ��6X   �� *	ImagePath+�� 5�   &   -   5        $��Center=�5�   0   $��12(�6a   ��6K   �� *Name+ � ContextMenu 5�   *   �� 5�   1   .@�5�   6   m� $��mniName��6M   -   5        $��修改名称��5�   ?   6\   m� 5�   6   $��	mniMoveUp��6M   -    5        $��上移节点��5�   ?   6^   m� 5�   5   $��mniMoveDown��6L   -!   5        $��下移节点��5�   >   6_   5�   3   5�   '   �� 5�   &   -"   5        $y�Hidden=�5�   .   .��6|   5�   '   5�   #     �� Triggers  5�   "   ^� 5�   &   ! ^� 
Conditions! 5�   *   �� 5�   .   " �� Binding" 68   �� # �� RelativeSource# �� $ �� AncestorType$$ TreeViewItemL�% �� Path$% 
IsExpanded�& �� Value& True6�   �� 5�   .   " 68   �� # �� $$ TreeViewItemL�$% HasItems�& True6�   .0�5�   *   �� ' �� 
TargetName$' imgTreeItem��61   ( �� Property( � ��) �� Value$,) &/Prospect;component/img/greenround.png��6\   5�   '   �� 5�   &   * �� Value$
* True��6�   + �� Binding+ 62   �� # �� $$ TreeViewItemL�$% 
IsSelected�.a�5�   *   �� $' imgTreeItem��61   ( � ��$,) &/Prospect;component/img/greenround.png��6\   5�   '   5�      5�      5�      5�      5�      � 5�   
   $��2��6    5�      �� 5�      $��* �6    6-   �� 5�      $
��Auto �6    60   .+�5�      �� -#   5        , ��Name$, ControlHostElement��5�      
�������$��3q�5      ���	#FF211D1D$��0��5     5     � 5     $��1��6    5     �� 5     $��20 �6$   62   �� 5     $��80 �6$   .+�5     �� $��1q�6   
�������
�������$��0��6Z   ._�5	     �  5
     �� 5     $
��Auto �6/   �� 5     $��* �6/   .+�5     �� $ 
Horizontal=�6%   $��0��6>   .+�5     �� -$   5        - ��Name$- sldPlay��5  %   $	��296��66   ���	#FFDCF7F96�   �� 5     -%   5        $��btnPlayVideo��5  %   $��30��6:   $ 播放选中的录像��6`   ��5  "   �� 5  &   $	 0,1&�6:   $	 0,0&�6I   .��5  *   � $ 	#FF92ADBA��67   $ 0Q�6I   � 5  *   $ 	#FFEBEBEB��67   $	 0.5Q�6I   � 5  *   $ 	#FFAAFFDE��67   $	 0.5Q�6I   � 5  *   $ 	#FF63BFEF��67   $ 1Q�6I   5  '   ��5  "   � 5  &   .+�5  *   �� $&�� /Prospect;component/img/play.png��60   $��19��6Z   $��	3,-5,1,-2q�6f   5  '   5     �� 5      -&   5        $��btnPauseVideo��5   %   $��30��6;   $ 暂停播放的录像��6b   ��5!  "   �� 5"  &   $	 0,1&�6:   $	 0,0&�6I   .��5#  *   � $ 	#FFC0DEED��67   $ 0Q�6I   � 5$  *   $ 	#FFEBEBEB��67   $	 0.5Q�6I   � 5%  *   $ 	#FFA9F3D6��67   $	 0.5Q�6I   � 5&  *   $ 	#FF63BFEF��67   $ 1Q�6I   5'  '   ��5)  "   � 5*  &   .+�5+  *   �� $'��!/Prospect;component/img/pause.png��60   5,  '   5.     �� 5/     -'   5        $��btnStopVideo��5/  %   $��30��6:   $ 停止播放的录像��6`   ��50  "   �� 51  &   $	 0,1&�6:   $	 0,0&�6I   .��52  *   � $ 	#FFC0DEED��67   $ 0Q�6I   � 53  *   $ 	#FFEBEBEB��67   $	 0.5Q�6I   � 54  *   $ 	#FFA9F3D6��67   $	 0.5Q�6I   � 55  *   $ 	#FF63BFEF��67   $ 1Q�6I   56  '   ��58  "   � 59  &   .+�5:  *   �� $&�� /Prospect;component/img/stop.png��60   5;  '   5=     �� 5>     -(   5        $��	btnImport��5>  %   $��30��67   $$ 导入外部图像或者录像��6Z   ��5?  "   �� 5@  &   $	 0,1&�6:   $	 0,0&�6I   .��5A  *   � $ 	#FFC0DEED��67   $ 0Q�6I   � 5B  *   $ 	#FFEBEBEB��67   $	 0.5Q�6I   � 5C  *   $ 	#FFA9F3D6��67   $	 0.5Q�6I   � 5D  *   $ 	#FF63BFEF��67   $ 1Q�6I   5E  '   ��5G  "   � 5H  &   .+�5I  *   �� $)��#/Prospect;component/img/addfile.png��60   5J  '   5L     �� 5M     	进度：6-   5N     � 5O     -)   5        . �Name$. pgbRecordTime��5O  E   $��1��6&   $	��100��66     ProgressBarStyle#��[ 6\   / � Minimum$/ 0Q�6�   0 � Maximum0 6�   �� 1 �� ElementName$1 tbxDuration��$
% Text�5Q     5R     �� 5S     $ 
Horizontal=�6   $��1��66   $��50��6C   $��0,1,0,29q�6R   .+�5T     �� -*   5        $��btnCaptureImage��5T  /   $ 快捷采集图像��6   $��80��6F   $��50��6Q   ��5U     �� 5V     $ 0.5,1&�62   $ 0.5,0&�6C   .��5W  "   � $ 	#FFD8F3E0��6/   $ 0.008Q�6A   � 5X  "   $ 	#FF8FF5AD��6/   $ 1Q�6A   � 5Y  "   $ 	#FFC3F3D1��6/   $ 0.492Q�6A   5Z     ��5\     � 5]     $��72��6#   .+�5^  "   �� $.��(/Prospect;component/img/caputreimage.png��6(   $��0,0,32,6q�6Z   �� 5_  "   $��44,3,2,4q�6,   抓取�� 6A   图像6P   5`     5b     �� 5d     -+   5        $��btnStartRecord��5d     $��80��65   $��50��6@   ��5e     �� 5f     $ 0.5,1&�62   $ 0.5,0&�6C   .��5g  "   � $ 	#FFD8F3E0��6/   $ 0.008Q�6A   � 5h  "   $ 	#FF8FF5AD��6/   $ 1Q�6A   � 5i  "   $ 	#FFC3F3D1��6/   $ 0.492Q�6A   5j     ��5l     � 5m     $��72��6#    5n  "   �� 5o  &   $	��30* �67   �� 5p  &   $	��42* �67   .+�5r  "   �� $(��"/Prospect;component/img/record.png��6(   $��	-4,4,32,6q�6T   $��2��6g   �� 5s  "   $��14,5,2,2q�6,   $��1��6>   开始�� 6Q   录像6`   5t     5v     �� 5w     -,   5        $��btnStopRecord��5w     $��80��64   $��50��6?   ��5x     �� 5y     $ 0.5,1&�62   $ 0.5,0&�6C   .��5z  "   � $ 	#FFD8F3E0��6/   $ 0.008Q�6A   � 5{  "   $ 	#FF8FF5AD��6/   $ 1Q�6A   � 5|  "   $ 	#FFC3F3D1��6/   $ 0.492Q�6A   5}     ��5     � 5�     $��72��6#   .+�5�  "   �� $,��&/Prospect;component/img/stoprecord.png��6(   $��	-4,4,32,6q�6X   �� 5�  "   $��44,5,2,2q�6,   停止�� 6A   录像6P   5�     5�     �� 5�     --   5        $��
btnSetting��5�     $��80��6I   $��50��6T   ��5�     �� 5�     $ 0.5,1&�62   $ 0.5,0&�6C   .��5�  "   � $ 	#FFD8F3E0��6/   $ 0.008Q�6A   � 5�  "   $ 	#FF8FF5AD��6/   $ 1Q�6A   � 5�  "   $ 	#FFC3F3D1��6/   $ 0.492Q�6A   5�     ��5�     � 5�     $��72��6#   .+�5�  "   �� $)��#/Prospect;component/img/setting.png��6(   $��	-4,4,32,6q�6U   �� 5�  "   $��	41,14,5,8q�6,   设置6C   5�     5�     � 5�     -.   5        2 �Name$2 
popSetting��5�     3 � 	StaysOpen3 .  4 � AllowsTransparency4 . 5 � HorizontalOffset$5 0��5�     6 � VerticalOffset$6 0��5�     $��Right=�5�     7 � PlacementTarget7 5�     �� $1 
btnSetting��.��5�     �� 
����ح�$	��160��6<   .+�5�     m� -/   5        $��mniCard��5�  6   $��采集卡设置��6'   6E   �� 5�     m� 5�     -0   5        $��	mniSystem��5�  6   $��标准库设置��6'   6G   �� 5�     m� 5�     -1   5        $��	mniOption��5�  5   $��选项设置��6'   6F   �� 5�     5�     5�     �� 5�     -2   5        $��btnPause��5�     $��80��6/   $��50��6:   ��5�     �� 5�     $ 0.5,1&�62   $ 0.5,0&�6C   .��5�  "   � $ 	#FFD8F3E0��6/   $ 0.008Q�6A   � 5�  "   $ 	#FF8FF5AD��6/   $ 1Q�6A   � 5�  "   $ 	#FFC3F3D1��6/   $ 0.492Q�6A   5�     ��5�     � 5�     $��72��6#   .+�5�  "   �� $(��"/Prospect;component/img/frozen.png��6(   $��	-4,4,32,6q�6T   �� 5�  "   $��	43,6,3,-3q�6,   冻结�� 6B   图像6Q   5�     5�     �� 5�     $��1q�6   
�������
�������._�5�     �� $��77��6"   $��40��6-   $��49��6<   $	��Top=�6H   ��5�     �� 5�  "   $ 0.5,1&�66   $ 0.5,0&�6G   .��5�  &   � $ 	#FFD8F3E0��63   $ 0.008Q�6E   � 5�  &   $ 	#FF8FF5AD��63   $ 1Q�6E   � 5�  &   $ 	#FFC3F3D1��63   $ 0.492Q�6E   5�  #   .+�5�     �� 8 �� Left$8 1��6(   
9 �� Top$9 6��68   $��12(�6G   $��时长:��6U   6b   �� 5�     -3   5        $	 tbxDuration��5�  &   $��14(�6;   $��10��6I   $��23��6S     TwoButtonTextBox#��[ 6_   $��73��6�   $8 1��6�   $9 22��6�   6�   �� 5�     -4   5        $��txbRecordTime��5�  (   $�� ��6=   
���  ��$��1��6V   $��52��6f   $8 29��6q   $9 6��6�   5�     5�     5�     5�     5�     5�     5�     