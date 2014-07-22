Twixel
======

An unofficial C# library for Twitch.TV

Official documentation can be found here: https://github.com/justintv/Twitch-API

Post Mortem (7/22/14)
=====
Even though the library isn't 100% finished yet I wanted to add this as what I learned when making this library. Today I realized that my library may be really wrong because I was looking at v2 documentation and accidentally implementing v3 code. With the Twitch API when you make a request it defaults to the latest version. V2 is listed as the stable version but v3 is the default version if no version is specified. This means when you look up API calls in browser (like this: https://api.twitch.tv/kraken/users/golf1052) you will get a v3 response. I noticed this when I was writing tests and I was getting a null value for my bio field in my library.

The library still works, I am currently working on an app that uses this library. What this does mean though is that at some point I should probably either update the library to support v3 fully, go back through and make sure I am only using v2, or add the option to use both. This may never happen but I would like to do it at some point.

I also didn't plan this library out at all. I just wanted a way to watch Twitch in Windows 8 on the metro side and maybe even watch Twitch on Windows Phone. So I found the Twitch API, started implementing API calls, and started working on the app. Because the library and the app were being developed at mainly the same time I made some changes that really were only applicable to my app instead of being general changes that could easily be used by everyone. One of the major ones was on [this commit](https://github.com/golf1052/Twixel/commit/715a412fb0f1fd0c1835f0b8be64681cece08614) when I changed most of the calls to keep running until they didn't get an HTTP status code. This was to fix a bug in my app that would cause the app to crash when an error from Twitch occurred.

I am currently in the process of reverting my mistake from my mentioned commit to fire an event when an HTTP status code happens and return a null value. The library also does not handle any Twitch errors (Twitch API errors not HTTP status codes from Twitch). I also mentioned I am adding tests that should make how the library functions clearer. Also at some point I will get my app up on GitHub as well.

I know that a couple people have looked at this library and I have even gotten a little feedback from someone at Twitch but if I was to use this library I know I would hate how it was built and having to deal with the random, undocumented changes that happen. It's a good learning experience and CreepScore (my League of Legends API library) takes some lessons from this one. I will get this library into a somewhat finished state that is useable by people and I will get my app out soon. Hopefully the next project I take on doesn't succumb to the same mistakes I made in this one.
