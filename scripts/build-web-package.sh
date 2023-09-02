ls
dotnet publish ./src/WhyNotLang.Blazor/ -o tmp
mkdir out
mv ./tmp/wwwroot/* out/
rm -rf tmp