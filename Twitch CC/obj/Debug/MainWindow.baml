   M S B A M L   `   `   `  ����D  @Twitch CC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Twitch_CC	Twitch_CC  5   	   ;"clr-namespace:Twitch_CC.ViewModelsTwitch_CC.ViewModels  5   	   1clr-namespace:Twitch_CC.ViewsTwitch_CC.Views  5   	       Twitch_CC.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Twitch_CC     5   	   9
viewmodels"clr-namespace:Twitch_CC.ViewModels     5   	   /viewsclr-namespace:Twitch_CC.Views     5   	   U QMaterialDesignThemes.Wpf, Version=3.1.0.981, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	      MaterialDesignBody#���   5
   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignFont#���  5   	   ���#29313E � Title$ 
MainWindow��5   	   $	��600��6   $
��1000��6)    � WindowStyle$
 None=�66    � WindowStartupLocation$ CenterScreen=�6I    � 
ResizeMode$ NoResize=�6n   .��5      �  � ColumnDefinitions 5   
   �� 5       � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      .+�5   
   ]� -   5        $��0��5      $��0��6!   ���#29313E.+�5      �� -   5        $��20��5      $��20��5      $��Right=�5       $��Right=�5!      $��0,0,5,0q�5"      ���#FF605C5%      �� 5&      -   5        $��20��5&      $��20��5'      $��Right=�5(      $��Right=�5)      $��0,0,10,0q�5*      ���#FFBD445-      �� 5.      $��Icon.ico��6   $
��Left=�6&   $��5,2,0,2q�6<   �� 5/      $
��Left=�6   $��Center=�50      $��Rubik)�51      $��20(�52      
�������$��30,1,0,0q�54      Stream Clipper for Twitch56      57       � 58   
   -   5          �Name$ 	MainFrame��58      	  � NavigationUIVisibility$	 Hidden=�59      ���#363636$��1��5;      $��0��5<      $��3q�5=      '  !MaterialDesignThemes.Wpf.Snackbar  5>   
   -   5        
  Name$
 snackBar��5>   "   $��1��5?   "   $��0��5@   "   5A      5B      