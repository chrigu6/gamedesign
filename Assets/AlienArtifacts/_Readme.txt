-------------------------
Alien Artifacts Kit (Lite)
by MobileJuze Pty Ltd
www.mobilejuze.com.au
-------------------------

DEMO SCENE: http://mojuze.com/assets_alienartifacts_demo.php
TUTORIAL: https://www.youtube.com/watch?v=fj8xCoBT-ic 
FORUM: http://forum.unity3d.com/threads/asset-wip-alien-artifacts-kit-lite-unique-customisable.275940/

README

1.
This "Alien Artifacts Kit" (Lite) has six base models, called Barth, DiPyramid, GeodesicA, Tetra, UWhat and Infinity

2.
The fastest way to get up and running is to check out the tutorial on YouTube at https://www.youtube.com/watch?v=fj8xCoBT-ic ...Alternatively just drag the prefabs from the "Prefabs" folder onto the scene.

3.
There are THREE kinds of textures: Clamped, Luminosity and Repeat. Basically for each base model, you can apply a variety of materials to them. Each material can use one of those textures. Here are the explanation of the textures:

Clamped:
These are UV-mapped textures (image and normal map) which correspond to a particular base model. That is, you cannot use a Barth clamped texture with an Infinity model. 

Luminosity:
These are UV-mapped textures (luminosity/ glow) which correspond to a particular base model. Similar to clamped textures you cannot use a Barth clamped texture with an Infinity model and so on. The goal of the luminosity texture is so that you can apply coloured additive, special emissive, etc. materials to the base model that does NOT affect the whole model. See the YouTube tutorial for more information.

Repeat:
These are several seamless textures (image and normal map) which you can apply to ANY base model. If the shader supports it you can tile these 1x1, 3x3, 10x100 or whatever you feel is suitable. The UV mapping of the base models is reasonable enough that you can experiment with your own seamless textures or seamless textures from other assets etc.

4.
Finally, in the example scene you can see how rotation might be applied to a game object, how you can "hide" and "unhide" different game objects, and dig into how the web player demo was made.

Enjoy and please do let me know your feedback.

Cheers,
srmojuze
MobileJuze Pty Ltd

