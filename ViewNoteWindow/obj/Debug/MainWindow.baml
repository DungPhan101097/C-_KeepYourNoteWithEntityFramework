   M S B A M L   `   `   `  ����I  EViewNoteWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ViewNoteWindowViewNoteWindow  5   	       ViewNoteWindow.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   (localclr-namespace:ViewNoteWindow  5   	   -   5        $��MainViewWindow��5	   	     � Title$  Keep My Notes��5
   	   $	��600��6   $	��900��6,    � 	Resources 5         NotifierContextMenu �� 	Placement��%�   &	        �� 5   
   $ 
MousePoint=�5      .@�5      m� -   5        $��
View notes��5      6+   m� 5      -   5        $��View Statistics��5      60   m� 5      -   5        $
��Exit��5      6%   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6   .+�5!   
   o� -   5        $
��menu��5!      $��0��5"      $��30��5#      $��Stretch=�5$      .@�5%      m� $
��File��6   $��Center=�5&      .@�5'      m� -   5        $��menuNewNote��5'   -   $��New note��6   6@   m� 5(      -   5        $��
menuNewTag��5(   ,   $��New tag��6   6>   m� 5)      -   5        $��menuViewAllNote��5)   3   $��View all Notes��6   6J   m� 5*      -	   5        $��menuViewAllTag��5*   2   $��View all tags��6   6g   m� 5+      -
   5        $��menuStatistics��5+   4   $��View statistics��6   6J   m� 5,      -   5        $��menuExit��5,   )   $
��Exit��6   69   5-      m� 5.      $��	_Tools...��6   $��Center=�5/      .@�50      m� $��View notes...��6   .@�51      m� -   5        $��	sortTitle��51   6   $��Sort by title��6   6b   m� 52      $��Sort by ascending date��6   6A   m� 53      $��Sort by descending date��6   6B   54      55      56      � 58   
   ���#666666$��1��5:       � ColumnDefinitions 5;      �� 5<      -   5         �Name$ FirstCollumn��5<   #   $	��250 �68   �� 5=      $��* �6#    5?      �� 5@      $	��150 �6    �� 5A      $��* �6    .+�5E      � $��10q�5F      
�������$��0��5G      $��0��6!    5H      �� 5I      $
��1.5* �6$   �� 5J      $
��3.5* �6$    5L      �� 5M      $��* �6'   �� 5N      $��70 �6'   .+�5Q      �� $��2��5R      $��2��5S      
���   �$��1q�5T   )    J� Effect 5U      3  -System.Windows.Media.Effects.DropShadowEffect  5V         	Direction$	 320Q�5W         
BlurRadius$ 5Q�5X      	   Color$	 Black��5Y      
   ShadowDepth$
 2Q�5Z         Opacity$	 0.5Q�5[      5]      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5_      -   5        $��txtSearchTag��5_   (   $��20,10,-10,0q�6<   $��0��5`   ,      	Watermark$ Search tag...��5a   ,   5b   /   �� 5c      -   5        $��btnSearchTag��5c      $��0��5d      $��1��6*   $��0, 10, 0, 0q�5e      $��Center=�5f      .��5g      �� $��./images/search.png��6   6<   5h      �� 5i      -   5        $��borderResult��5k      $��1��5i      $��20,10,-10,10q�6&   
���   �$��1q�5j   1   $y�	Collapsed=�5k   1   ._�5l      �� $
��Auto=�6#   .��5m      �� -   5        $��resultStack��5m   %   6:   5n      5o      5p      �� 5t      $��1��5u      $��0��5v      $��2��6"   $��Visible=�5w      $
��Auto=�5x      $��
0,10,10,10q�5y      .��5z      � -   5        $��content��5z      5|      5}      � 5�      $��0��5�      $��1��6!   $��
10,0,10,10q�5�      
�������.+�5�      �� 
���   �$��1q�6-    5�        5�      $	 320Q�5�      $ 5Q�5�      $	 Black��5�      $
 2Q�5�      $	 0.5Q�5�      5�      R� 5�      -   5        $��	tvListTag��5�      6,   5�      5�      5�      5�      