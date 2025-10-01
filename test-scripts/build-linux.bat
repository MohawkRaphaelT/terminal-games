echo on

pushd

:: Delete old builds folder and contents
:: /s	Deletes a directory tree (the specified directory and all its subdirectories, including all files).
:: /q	Specifies quiet mode. Does not prompt for confirmation when deleting a directory tree.
::      The /q parameter works only if /s is also specified.
rmdir /s /q builds

:: Move to project folder
cd ../vs-launch-external-terminal

:: Build all targets
dotnet publish -r linux-x64 -o ../builds/linux-x64

:: Go back to root folder
popd

:: Compress folders as ZIP
::pushd .
::cd builds/

::tar.exe -a -c -f	linux-x64.zip	linux-x64

::popd

:: For debugging
:: PAUSE