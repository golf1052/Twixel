# Twixel Documentation

This documentation is built using [docfx](http://dotnet.github.io/docfx/).

## How to Build and Deploy Said Documentation

1. `git checkout gh-pages`
2. `docfx build Documentation/docfx.json`
3. `cp -r Documentation/_site/* .`
4. `git commit -m "updated documentation"`
5. `git push`
