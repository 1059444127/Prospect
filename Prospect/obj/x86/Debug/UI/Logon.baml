   M S B A M L   `   `   `  ����  Prospect#    com.cloud.prospect.XAML.Logon   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     6@   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6}   $��12(�5   	     � ShowInTaskbar  .   � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�6    � Title$ Logon��5   	   $	��600��6   $	��850��6$    � WindowStartupLocation$ CenterScreen=�60   ��5      �� 5   
    �� ImageSource$' !/Prospect;component/img/logon.jpg��6    j� Stretch$
 Fill=�6E    j� TileMode$
 None=�6T   .��5
      � .+�5   
   �� -   5         ��Name$ label1��5   Y   $��科室��6   $��28��6   $
��Left=�6)   $��227,219,0,0q�6D   $	��Top=�6i   	 J� RenderTransformOrigin$	 0.588,-0.214&�6�   �� 5   
   -   5        $ label2��5   Y   $��用户��6   $��28��6   $
��Left=�6)   $��227,248,0,0q�6D   $	��Top=�6i   �� 5   
   -   5        
 ��Name$
 cbxDepartment��5   N    �� 
IsEditable .  �� 
IsReadOnly . $��272,219,401,0q�67   $	��Top=�6�   �� 5   
   -   5        $
 	cbxDoctor��5   N    .  . $��272,248,401,0q�67   $	��Top=�6a   �� 5   
   -   5        $ label3��5   O   $��密码��6   $
��Left=�6   $��227,276,0,296q�68   $��34��6_   G� 5   
   -   5         G�Name$ 	pwdDoctor��5   /   $��272,277,401,300q�6   6/   �� 5   
   -   5         ��Name$ btnLogon��5   d   $��23��6   ���	#FF6AABE5$
��Left=�64   $��220,0,0,266q�6O   $��Bottom=�6v   $��53��6�   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $)��#/Prospect;component/img/patient.png��6   �� 5      �� 6!   $�登陆��6%   62   5      5      �� 5   
   -   5        $ btnClose��5   e   $��23��6   ���	#FF6AABE5$
��Left=�65   $��277,0,0,266q�6P   $��Bottom=�6w   $��57��6�   .��5      �� $ 
Horizontal=�6   .+�5      �� $(��"/Prospect;component/img/close2.png��6   �� 5      �� 6!   $�退出��6%   62   5      5      �� 5      -	   5         ��Name$ tbkRegister��5      $��软件注册...��6#   $��394,0,397,266q�62      styleHyperlink#��[  6I   $��15��6�   $��Bottom=�6�   $	 7.258,2.958&�6�   �� 5   
   -
   5        $ btnChangePassword��5   �   $��修改密码��6   ���	#FF6AABE5���	#FFD4B9B9$��23��6O   $
��Left=�6[   $��338,0,0,266q�6v   $��Bottom=�6�   $��53��6�   �� 5   
   -   5        $��lblRegistered��5   Z   $��	已授权��6   $��29��6   $
��Left=�6*   $��391,0,0,257q�6E   $��Bottom=�6o   $��62��6�   $y�Hidden=�6�   5       5!      