   M S B A M L   `   `   `  ����N  JCodeMobileChallenge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CodeMobileChallenge.ViewsCodeMobileChallenge.Views  5   	   I)clr-namespace:CodeMobileChallenge.HelpersCodeMobileChallenge.Helpers  5   	   )    #CodeMobileChallenge.Views.LoginView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:CodeMobileChallenge.Views     5   	   <helper)clr-namespace:CodeMobileChallenge.Helpers     5   	     N� Title$  Login��5	   	   $	��450��6   $	��800��6$   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��2��6!   $��Login��65   $��24(�6B   $
��Bold"�6P   $��10q�6b   $��Center=�6n   �� 5   
   $��1��6   $��0��6!   $��10q�61   $��Center=�6=   $��Email:��6X   �� 5   
   $��1��6   $��1��6   $��10q�6/   ��6;   �� *Email+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��2��6   $��0��6!   $��10q�61   $��Center=�6=   $��	Password:��6X   G� 5   
   $��2��6   $��1��6#   $��10q�63   3   -CodeMobileChallenge.Helpers.PasswordBoxHelper   BindPassword .    BoundPassword 5      �� *
Password+ �� Mode$ TwoWay=��� 5!   
   $��3��6   $��1��6   $��10q�6.   $��Login��6:   $��Right=�6k   ��6J   �� *LoginCommand+6k   �� 5#   
   $��4��6   $��2��6!   $��10q�65   
���  ����6R   �� *ErrorMessage+5$      5%      