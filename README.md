# Chronotator
Play around with temporal rotation of videos in real time.

What is temporal rotation?
- Take a video
- Think of each frame as a square
- Space out those squares to form a cube
- Playing the video is analogous to sweeping a plane through from the front of the cube to the back
- Temporal rotation is rotating this plane so that it hits multiple frames in different places
- This results in images where different regions are from different times in the video

## Example

*Some flamingos:* <br/>
<img src="https://user-images.githubusercontent.com/43935094/182721805-8248f887-5c41-4d88-884a-861c4fc70e3a.PNG" alt="drawing" height="300"/>

*45 degree horizontal rotation:* <br/>
*The left hand side of the image is at the start of the video, the right hand side is at the end.* <br/>
<img src="https://user-images.githubusercontent.com/43935094/182721818-243c321e-d6a5-4062-8553-2ac994441df3.PNG" alt="drawing" height="300"/>

*90 degree horizontal rotation:* <br/>
*One column of pixels changing through time.* <br/>
Flamingos that moved across this column over the video are somewhat recogniseable. <br/>
<img src="https://user-images.githubusercontent.com/43935094/182721830-9b609b38-c10d-4ffe-9371-d274f0530e04.PNG" alt="drawing" height="300"/>

*45 degree vertical rotation:* <br/>
*The top of the image is at the start of the video, the bottom is at the end.* <br/>
<img src="https://user-images.githubusercontent.com/43935094/182721822-585f9291-a7cb-408b-a3d1-82fe9330187e.PNG" alt="drawing" width="400"/>

*90 degree vertical rotation:* <br/>
*A tangle of necks and legs - one row of pixels changing through time.* <br/>
<img src="https://user-images.githubusercontent.com/43935094/182721839-7f054fe8-c32f-4ef9-9ff3-dff6ebbd3a3d.PNG" alt="drawing" width="400"/>

### Sample Video Sources:
- [Cow](https://pixabay.com/videos/highlander-cattle-animal-mammal-125147/)
- [Fire](https://pixabay.com/videos/fire-water-elements-burn-flame-125068/)
- [Fish](https://pixabay.com/videos/fish-underwater-aquarium-swim-110879/)
- [Flamingos](https://pixabay.com/videos/flamingo-water-bird-bird-exotic-120394/)
- [Flower](https://pixabay.com/videos/cornflower-flower-blossom-bloom-124429/)
- [Tunnel](https://pixabay.com/videos/tunnel-stars-light-exit-man-124983/)
