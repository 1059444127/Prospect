   M S B A M L   `   `   `  ����C  ?Prospect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:com.cloud.prospectcom.cloud.prospect  5   	   $    com.cloud.prospect.ProspectWin   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :Custom-http://schemas.microsoft.com/wpf/2008/toolkit  5   	   X TXceed.Wpf.Toolkit, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   ,local clr-namespace:com.cloud.prospect  5   	   -   5          �Name$  Window��5
       � Title$0 *北京华明雷德科技有限责任公司��5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�6    J� SnapsToDevicePixels . #��� ��5      #��� ��5       � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�5       � Icon$&  /Prospect;component/img/logo.png��6I    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5        %   com.cloud.prospect.UserCommands   DeleteImage	   Command#	 Z 5      5   2   .��5      � -   5        
 �Name$
 
LayoutRoot��5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      -   5         ��Name$ ControlHeight��5      $��* �63   �� 5      $
��Auto �6   .+�5   
   � -   5        $
 topGrid��5      $��0��6    .+�5      �� $��63��6   $	��Top=�6$   ��5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ 	#FF628FB2��6'   � 5       $ 	#FFD6E7F9��6'    � Offset$ 1Q�69   5!      ��5#      �� 5$       �� Color# � ��6&   5&      �� 5'      -   5         ��Name$ 	btnSearch��5,   /   $
��Left=�5'      $	��Top=�5(      $��50��5)      $��65��5*      $��8,8,0,0q�5+         ToolButtonStyle#��[  5,       � ToolTip$$ 设置条件搜索病人病例��6Z   .��5-      ��  �� Orientation$ Vertical=�6   .+�5.      �� $��50��6   $��28��6'   $��img/search.png��63   �� 5/      $��Center=�6    $��12(�6;   查找病例6O   50      51      �� 52      -   5        $ btnNewPatient��54   3   $	��Top=�52      $��50��53   
   $��65��6   $��75.25,7.75,0,0q�6!   #��[  54   
   $ 新增一个病人病例��6f   $
��Left=�6y   .��55      �� $ Vertical=�6   .+�56      �� $��50��6   $��28��6'   $��img/patient.png��63   �� 57      $��Center=�6    $��12(�6;   �� 6J   $�新增病例��6N   6]   58      59      �� 5:      -   5        $ btnSave��5@   3   $
��Left=�5;   
   $	��Top=�5<   
   $��50��5=   
   $��65��5>   
   $��	142,8,0,0q�5?   
   #��[  5@   
   $ 保存当前病例信息��6Z   .��5A      �� $ Vertical=�6   .+�5B      �� $��50��6   $��28��6'   $��img/save.png��63   �� 5C      $��Center=�6    $��12(�6;   �� 6J   $�保存信息��6N   6]   5D      5E      �� 5F      -	   5        $ Update��5H   3   $	��Top=�5F      $��50��5G   
   $��209.25,7.75,0,0q�6   #��[  5H   
   $ 编辑当前病例信息��6X   $
��Left=�6k   .��5I      �� $ Vertical=�6   .+�5J      �� $��50��6   $��28��6'   $��img/editor.png��63   �� 5K      $��Center=�6    $��12(�6;   �� 6J   $�编辑信息��6N   6]   5L      5M      �� 5N      -
   5        $ 	btnDelete��5T   3   $
��Left=�5O   
   $	��Top=�5P   
   $��50��5Q   
   $��65��5R   
   $��267.25,8,0,0q�5S   
   #��[  5T   
   $$ 删除当前病例所有信息��6^   .��5U      �� $ Vertical=�6   .+�5V      �� $��50��6   $��28��6'   $��img/delete.png��63   �� 5W      $��Center=�6    $��12(�6;   �� 6J   $�删除病例��6N   6]   5X      5Y      �� 5Z      -   5        $ btnReportTemplate��5`   I   $
��Left=�5[   
   $	��Top=�5\   
   $��50��5]   
   $��65��5^   
   $��334.5,8,0,0q�5_   
   #��[  5`   
   $' !调取数据库中的诊断模板��63   .��5a      �� $ Vertical=�6   .+�5b      �� $��50��6   $��28��6'   $��img/reporttemplate.png��63   �� 5c      $��Center=�6    $��12(�6;   �� 6J   $�报告模板��6N   6]   5d      5e      �� 5f      -   5        $��btnPrintReport��5k       $
��Left=�5g   
   $	��Top=�5h   
   $��50��5i   
   $��65��5j   
   $$ 打印当前病例图文报告��6   $��402,7.75,0,0q�5k   
   #��[  5l   
   .��5m      �� $ Vertical=�6   .+�5n      �� $��50��6   $��28��6'   $��img/printer.png��63   �� 5o      $��Center=�6    $��12(�6;   �� 6J   $�打印报告��6N   6]   5p      5q      �� 5r      -   5        $��btnFirstPatient��5x   3   $
��Left=�5s   
   $	��Top=�5t   
   $��50��5u   
   $��45��5v   
   $��	467,8,0,0q�5w   
   $* $定位数据库中首位病例信息��6   #��[  5x   
   .��5y      �� $ Vertical=�6   .+�5z      �� $��50��6   $��28��6'   $��img/first.png��63   �� 5{      $��Center=�6    $��12(�6;   �� 6J   $�首  例��6N   6]   5|      5}      �� 5~      -   5        $��btnLastRecord��5�   3   $
��Left=�5   
   $	��Top=�5�   
   $��50��5�   
   $��45��5�   
   $��510.75,8,0,0q�5�   
   $- '定位数据库中上一位病例信息��6    #��[  5�   
   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/previous.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�	上一例��6N   6\   5�      5�      �� 5�      -   5        $��btnNextRecord��5�   3   $
��Left=�5�   
   $	��Top=�5�   
   $��50��5�   
   $��45��5�   
   $��	556,9,0,0q�5�   
   $- '定位数据库中下一位病例信息��6   #��[  5�   
   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/next.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�	下一例��6N   6\   5�      5�      �� 5�      -   5        $��btnLatestRecord��5�   -   $
��Left=�5�      $	��Top=�5�      $��50��5�      $��45��5�      $��	601,8,0,0q�5�      $* $定位数据库中末位病例信息��6   #��[  5�      .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/end.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�末  例��6N   6]   5�      5�      �� 5�      -   5        $��	btnStatic��5�   +   $
��Left=�5�      $	��Top=�5�      $��50��5�      $��65��5�      $��	650,8,0,0q�5�      $ 显示综合统计信息��6   #��[  5�      .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/stats.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�综合统计��6N   6]   5�      5�      �� 5�      -   5        $��btnMultipleThread��5�   3   $
��Left=�5�   
   $	��Top=�5�   
   $��50��5�   
   $��715,8,60.498,0q�5�   
   $E ?写当前病人信息时，可以采集下一位病人的图像��6#   #��[  5�   
   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/thread.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�多 线 程��6N   6^   5�      5�      �� 5�      -   5        $ 	btnReturn��5�   K   $��Right=�5�   
   $	��Top=�5�   
   $��50��5�   
   $��45��5�   
   $��	0,8,107,0q�5�   
   $y�Hidden=�67   #��[  5�   
   $��1��63   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $(��"/Prospect;component/img/return.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�返  回��6N   6]   5�      5�      �� 5�      -   5        $��Right=�5�   
   $	��Top=�5�   
   $��50��5�   
   $��45��5�   
   $��0,8,57,0q�5�   
   #��[  5�   
   $��1��63   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/minus.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�	最小化��6N   6\   5�      5�      �� 5�      -   5        $��btnHelp��5�   3   $
��Left=�5�   
   $	��Top=�5�   
   $��50��5�   
   $��45��5�   
   $��	775,8,0,0q�5�   
   #��[  5�   
   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/help.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�帮  助��6N   6]   5�      5�      �� 5�      -   5        $��Right=�5�   
   $	��Top=�5�   
   $��50��5�   
   $��45��5�   
   $��0,8,8,0q�5�   
   #��[  5�   
   $��1��63   .��5�      �� $ Vertical=�6   .+�5�      �� $��50��6   $��28��6'   $��img/close.png��63   �� 5�      $��Center=�6    $��12(�6;   �� 6J   $�退  出��6N   6]   5�      5�      5�      � 5�   
   $��1��6   ���	#FFDDF0F9 � ColumnDefinitions 5�      �� 5�      $��* �6#   �� 5�      $
��Auto �6#   .+�5�      �� -   5        $��tc��5�   -   $��0��6   $	��Top=�67   �� 5�      -   5         ��Name$ scrolls2��5�      $��1��6-   $
��Auto=�6>   $
��Auto=�6a   ��5�      �� 5�      -   5         ��Name$ 	stkImage2��5�      {�.  ���	#FFD8F9EB��5�      �� 5�      .R�5�   "   �  � Orientation$ Vertical=�6,    K� IsItemsHost . 5�   #   5�      5�      5�      5�      �� 5�   
   -   5        $ scrolls��5�   2   $��Bottom=�6   $��2��6D   $	��100��6Q   $
��Auto=�6_   $
��Auto=�6�    � ContextMenu 5      �� 5     -   5        $��cm��5     .@�5     m� -   5        $��muiPaste��5  +   $��粘贴��6   6;   5     ��5     �� 5     -   5        $ stkImage��5      �� SelectionMode$ Extended=�6-   ���	#FFD8F9EB � 	Resources 5      �� 
TargetType �� Property �� Value  �� 
TargetType! �� Padding" �� Triggers# P� Property$ P� Value% �� 
TargetName��%�  '��       (�� ;    5  *   * SystemColors.HighlightBrushKey+)�� 5     " ��6    .�5	     ��  � �� 5
  "   �� 5  &   "  ��66   .R�5  *   �� $��Border��61   $! 1q�6?    . ._�5  .   �� 5  +   " 5  *   P� 5  .   # �@��
IsSelected$ . . �5  2   �� $% Border��69   # � ��5  9    � ��5  /   5  '   5     5     ��5     Red��6a   5     ��5     �� 5     .R�5     � $ 
Horizontal=�6(    . 5      5!     5#     5&     5'     5(     