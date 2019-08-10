dotnet publish src/WhyNotLang.Web/ -o tmp
mkdir artifacts
mv ./tmp/WhyNotLang.Web/dist/* artifacts/
rm -rf tmp