   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   C ?Prospect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:com.cloud.prospectcom.cloud.prospect 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 6q   (   "com.cloud.prospect.StatisticWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   2local clr-namespace:com.cloud.prospect     5   	   6my-http://schemas.microsoft.com/wpf/2008/toolkit  68   0my1&clr-namespace:System;assembly=mscorlib  6q   :d2http://schemas.microsoft.com/expression/blend/2008  6�   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  6�   -   5          � ShowInTaskbar  .   � WindowState$ 	Maximized=�5   	   ���	#FFDDF0F9 � Title$ ProspectReport��5   	   $	��768��6    $
��1280��6-    � Icon$&  /Prospect;component/img/logo.png��6S   .��5	      � ���	#FFB1CFFF � RowDefinitions 5
   
   �� 5      $��80 �6   �� 5      $��* �6   .+�5   
   � $��1��6    � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5      $��* �6#   �� 5      $	��150 �6#   .+�5      7  1System.Windows.Forms.Integration.WindowsFormsHost  $��1��6      Child. 5      &   com.cloud.prospect.ReportControl  -   5        ucReport��5   &   5      � 5      $��0��6    5      �� 5      $	��364 �6$   �� 5      $��* �6$   .+�5      � .+�5      �� -   5         ��Name$ 	lbxReport��5   W   $	��329��6   $
��Left=�6+   $��6,0,0,0q�6F   $	��Top=�6j   $	��188��6�   6�   �� 5      -   5         ��Name$ btnDesignReport��5   h   $��设计报表��6   $��23��6,   $
��Left=�68   $��119,335,0,0q�6S   $	��Top=�6�   $��75��6�   ���	#FFD1F0FF6�   5      � 5       $��1��6   5!      � 5"      $��2��6   .+�5#      �� 	 �� Orientation$	 Vertical=�6   .+�5$      �� $��20��6    $��20��6X   �� 6e   $�开始时间��6i   6y   +  %Microsoft.Windows.Controls.DatePicker  5%      -   5        
  Name$
 dteStart��5%   $   
 �� Now   SelectedDate# Z 66   $	��126��6a   $��24��6n   $	��Top=�6}   �� 5&      $��20��6!   �� 6Z   $�结束时间��6^   6n     5'      -   5        $
 dteEnd��5'   %   # Z 65   $	��126��6`   $��24��6l   $	��Top=�6{   �� 5)      -   5        $ 
btnRefresh��5)   8   $��刷新报表��6   $��23��6,   $��75��6L   6W   5*      5+      5,      � 5-   
   $��0��6   .+�5.      �� -   5         ��Name$ image1��5.   N   $��60��6   $
��Left=�6    $��	12,12,0,0q�6;   $
��Fill=�6^   $	��Top=�6m   $��62��6�   $��2��6�   $&�� /Prospect;component/img/logo.png��6�   �� 5/      -	   5         ��Name$ 
textBlock1��5/   g   $��24(�6   $
��Bold"�6&   $
��Left=�69   $��	90,12,0,0q�6T   $��ProspectReport��6{   $	��Top=�6�   $	��194��6�   �� 50      -
   5        $ 
textBlock2��50   V   $��13(�6   $
��Left=�6'   $��
100,49,0,0q�6B   $��version 1.0.0��6j   $	��Top=�6   $	��116��6�   51      52      53      