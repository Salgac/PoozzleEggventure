using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelController : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI brownText;
    public Button menuButton;
    public Button nextButton;
    public LevelManager manager;

    private readonly List<string> splashes = new List<string>
    {
        "Oops! Time's up, and the egg couldn't quite make it to the toilet in time. Eggstremely close, though!",
        "Uh-oh! The egg missed the deadline for the toilet dash. A little eggciting mess to clean up!",
        "Time's ticking, and the egg missed the mark! Looks like a hilarious egg-mergency cleanup!",
        "Oh no! The egg had a time management mishap and missed the toilet. Eggstreme urgency!",
        "Yikes! The egg's schedule got scrambled, and it missed the toilet appointment. Eggstremely funny fumble!",
        "Eggcuse me! The egg got caught up and missed the toilet rendezvous. An eggceptional comedy of timing!",
        "The clock struck zero, and the egg missed its toilet mission. Eggstremely amusing, but a bit messy!",
        "Whoops! The egg didn't quite beat the clock to the toilet. Eggstremely close call, though!",
        "Time's toilet-tally up! The egg didn't make it in time. An eggstraordinary misadventure!",
        "Uh-oh! The egg's time management skills cracked, and it missed the toilet. Eggstremely humorous hiccup!",
        "Tick-tock, egg couldn't beat the clock! A little mess, a lot of eggcitement!",
        "Egg-streme lateness! The toilet waited, the egg hesitated. A comedy of eggrrors!",
        "Time's up, and the egg missed the bowl! An eggstraordinary twist in the plot!",
        "Late for the egg-sclusive toilet party! Cleanup on aisle egg – eggstreme hilarity!",
        "The egg's comedy of errors: missed the toilet cue! Eggstremely close shave!",
        "Oh no, egg missed the call of nature! Eggstremely funny, but also eggstremely messy!",
        "Toilet ETA missed! Egg's sense of timing needs a little work. Eggstremely entertaining failure!",
        "Egg-splosive surprise! Time's out, and the egg missed the porcelain throne. Eggstremely funny chaos!",
        "Egg-tastrophe! The egg missed its bathroom debut. An eggceptional blooper!",
        "Late for the party, egg missed the toilet fiesta! An eggstraordinary mix-up!",
        "Error 404: Toilet not found! The egg's quest for the porcelain kingdom hit a runtime exception.",
        "Oops! Looks like the egg encountered a race condition and lost the toilet sprint.",
        "Unexpected output: egg missed the toilet deadline. Debugging required for this eggceptional mishap.",
        "Unhandled exception in the toilet time code! The egg's attempt to flush with success failed.",
        "Time complexity exceeded! The egg couldn't compute the toilet arrival within the allotted timeframe.",
        "Buffer overflow! The egg's journey to the toilet exceeded the allocated space.",
        "Null pointer exception: the egg missed the target. Looks like a toilet reference was lost.",
        "Syntax error in toilet navigation! The egg's attempt to parse the bathroom failed.",
        "Code execution halted: egg didn't make it in time. Looks like a runtime toilet error.",
        "Eggception caught: toilet access denied! The egg's permission to enter was revoked."
    };

    // Start is called before the first frame update
    void Start()
    {
        menuButton.onClick.AddListener(MenuClicked);
        nextButton.onClick.AddListener(NextClicked);
    }

    // Update is called once per frame
    void Update() { }

    public void SetMenuTexts(int level, int moves, bool brown)
    {
        mainText.text = $"Level {level} Complete!";
        movesText.text = $"Moves: {moves}";

        if (brown)
        {
            brownText.gameObject.SetActive(true);
            brownText.text = splashes[Random.Range(0, splashes.Count - 1)];
        }
    }

    private void MenuClicked()
    {
        manager.FadeToLevel("MainMenu");
    }

    private void NextClicked()
    {
        manager.FadeToLevel();
    }
}
