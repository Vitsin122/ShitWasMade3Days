   M S B A M L   `   `   `  ����B  >Kursach, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:KursachKursach  5   	       Kursach.ViewWin   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Kursach     5   	     � Title$  ViewWin��5   	   $	��450��6   $	��800��6&   .��5	      � ���#024073.+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgSearchCrimes��5
         AutoGenerateColumns$ False��6+   $��216,0,216,0q�6G   $��0q�6\      Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$4 .Все найденые преступники��6)      CellTemplate 5      �� 5      .R�5      � ���#024073 � ColumnDefinitions 5   "   �� 5   &   �� 5   &   .+�5   "   �� ���#024073$��0��6B   .+�5   &   �� $	��140��6M   $	��180��6Y   $��5q�6f   ��6,   ��  �� Path$ _bytes�6f   �� 5   &    ��Name$ SearchDosie��6-   -   5        $��25��5   B   $��90��6N   $��
Досье��6Y   	 � 	Resources	 5   *   
 �� 
TargetType �� Property �� Value��%_   '��       �� 5   .   "
 ��64   .�5   2   ��  �@��CornerRadius$ 10��6Q   5   /   5   +   5   '   5   #   �� 5   "   ���#024073$��1��6B   .+�5   &   �� 
�������$��Имя:��6?   �� 5    &   $��5q�6-   
������� �� CornerRadius$ 10��6K   ._�5!   *   �� 
�������$��5q�6e   ��6G   �� $ _name�6e   5"   '   �� 5#   &   
�������$��Фамилия:��6?   �� 5$   &   $��5q�6-   $��1q�68   
�������$ 10��6_   ._�5%   *   �� 
�������$��5q�6i   ��6G   �� $ 	_lastname�6i   5&   '   �� 5'   &   
�������$.��(Преступная профессия:��6?   �� 5(   &   $��5q�6-   $��1q�68   
�������$ 10��6_   ._�5)   *   �� 
�������$��5q�6j   ��6G   �� $ 
_crimeprof�6j   5*   '   5+   #   5,      5.      50         	CellStyle 52      �� 53      *  $System.Windows.Controls.DataGridCell"
  6   .�54      ��  � ��$ 5q�60   �� 55       � �� 56      �� 57       �� 
TargetType"  6.   .R�58   "   ��  �� Padding# z��6)   #��z��6M   #��z��6y   #��z��6�    J� SnapsToDevicePixels$
 True��6�   ._�59   &   �� # z 67   5:   #   5;      5=      5>      5@      5A      5B      