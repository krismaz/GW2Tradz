from utils import gold

budget = 10000 * gold  # Maximum gold spend, slightly increases runtime
simplicity = 10  # How many lines of output / distinct operations, keep this low~ish for speed, or just very large for omegaspeed
days = 7  # How many days of buy/sell are you fetching?
days_tag = "7d"  # How many days of buy/sell are you fetching?
hours =  12 / 24  # How much of daily buy/sell can you get
sanity = 50000  # How much of one single thing can we do, effectively limits buys
safetyprice = 1.0  # Adjust sell prices slightly, this helps prevent silly 1-2% flips that might technically be optimal, but are rarely great in practice
# Do not try to sell and item if we can't sell for more than x gold a day, higher means more speed but less flexibility
min_move_per_day = 10 * gold
min_velocity = 5  # Do not try to buy/sell an item if we can get less than this amount of it in the alotted time, higher means more speed but less flexibility
click_weight = 100  # Up this if you want less lines but larger chunks

solveroptions = {"gapRel": 0.01,
                 "timeLimit": 240.0}  # Options for the cbc solver

# put your own api key here obvs
apikey = 'D29B3875-547F-2B47-AE06-961A0E3AB053D9799A10-D18F-4D75-8603-6BC8176EEB87'
