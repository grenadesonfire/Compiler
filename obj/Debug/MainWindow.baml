   M S B A M L   `   `   `  ����C  ?Compiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CompilerCompiler  5   	       Compiler.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Compiler     5   	     � Title$  Compiler��5   	   $	��600��6   $	��800��6'    � WindowStartupLocation$ CenterScreen=�63   .��5	      � .+�5
   
   o� $��0q�6   $	��Top=�6   .@�5      m� $��_File��6   .@�5      m� -   5        $��_Open...��5      6-   m� 5      -   5        $��_Save...��5      6-   �� 5      m� 5      $
��Exit��6   5      m� 5      $��_Compile��6   .@�5      m� -   5        $��	_Tokenize��5      6.   m� 5      -   5        $��_Parse��5      6+   5      5      �� 5   
   -   5         ��Name$ 
tabControl��5      $��0,26,0,0q�6)   $	��546��6;   .@�5      �� $��Current File��6   .��5      � ���	#FFE5E5E5.+�5      �� -   5         ��Name$ JackFile_TextBox��5       �� TextWrapping$
 Wrap=�68   $�� ��6L    �� 
IsReadOnly . 
���   ����	#FF37DA076y   5      5      �� 5      $��Tokenizer Results��6   .��5      � ���	#FFE5E5E5.+�5      �� -   5        $ Token_TextBox��5      $
 Wrap=�65   $�� ��6I    . 
���   ����	#FF18ACE46v   5       5!      �� 5"      $��Parser Results��6   .��5#      � ���	#FFE5E5E5.+�5$      �� -   5        $ Parsed_TextBox��5$      $
 Wrap=�66   $�� ��6J    . ���	#FF5B5B5B���	#FF18ACE4���	#FFA3B43A6�   5%      5&      5'      �� 5(   
   $
��Left=�6   $��20��6/   $��	0,548,0,0q�6;   $	��Top=�6N   $	��800��6f   .@�5)      �� -	   5         ��Name$ StatusLabel��5)      $��Label��6)   $	��150��69   5*      5+      5,      