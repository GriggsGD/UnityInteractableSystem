# 10/11/21 

I had an issue with the player going through 2 interactable zones positioned next to eachother. I presumed when the player entered the second trigger zone from the first it set the next interactable to the player with the first interactable clearing it shortly after leaving it null as the current interactable zone is only set on entering, to fix this I used as OnTriggerStay method to make sure the current interactable is set and added a function to check if theres an interactable set on the player already to prevent overwriting.
