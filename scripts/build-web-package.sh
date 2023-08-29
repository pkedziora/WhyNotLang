ls
dotnet publish ./src/WhyNotLang.Blazor/ -o tmp
mkdir out
mv ./tmp/WhyNotLang.Blazor/dist/* out/
rm -rf tmp