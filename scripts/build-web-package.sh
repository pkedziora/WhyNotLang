dotnet publish src/WhyNotLang.Web/ -o tmp
mkdir out
mv ./tmp/WhyNotLang.Web/dist/* out/
rm -rf tmp