   M S B A M L   `   `   `  ����D  @MiniGames, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MiniGames	MiniGames  5   	       MiniGames.ModalWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:MiniGames     5   	     � Title$  ModalWindow��5   	   $	��200��6   $	��400��6*   $	��200��66   $��99��6E    � AllowsTransparency .  � WindowStyle$
 None=�6n    � 
ResizeMode$ NoResize=�6�    � WindowStartupLocation$ CenterScreen=�6�   ��5
      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 0,1&�6/   .��5      �  � Color$ 	AliceBlue��6    � Offset$	 0.4Q�6-   � 5      $
 Aqua��6   $ 1Q�6(   5      .��5      � -   5        	 �Name$	 RootGrid��5      
 � RowDefinitions
 5   
   �� 5      $��30 �6   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ CloseButton��5      $��Resources/closeButton.png��5      $��Right=�5       � Cursor$
 Hand��5      �� 5   
   -   5         ��Name$ 	ModalText��5      $��10q�5      $
��Wrap=�5      $��1��5      $E��?/MiniGames;component/Resources/Font/#Earth's Mightiest Cyrillic)�5      $��Center=�6a   $	��Top=�6~   $��42(�6�    �� TextAlignment$ Center=�6�   Какой-то текст5!      5"      5$      