ECHO Building Skin Files...

del ..\..\..\..\Skins\*.skin

..\..\..\..\Tools\7-zip\7z.exe a -tzip -mx9 -r -x!Addons "..\..\..\..\Skins\Default.skin" ".\Content\Skins\Default\*.*"
..\..\..\..\Tools\7-zip\7z.exe a -tzip -mx9 -r -x!Addons "..\..\..\..\Skins\Green.skin" ".\Content\Skins\Green\*.*"



