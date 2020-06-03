from utils import gold

budget = 3000 * gold  # Maximum gold spend, slightly increases runtime
simplicity = 50  # How many lines of output / distinct operations, keep this low~ish for speed, or just very large for omegaspeed
days = 7  # How many days of buy/sell are you fetching?
days_tag = "7d"  # How many days of buy/sell are you fetching?
hours = 6 / 24  # How much of daily buy/sell can you get
sanity = 25000  # How much of one single thing can we do, effectively limits buys
safetyprice = 0.97 # Adjust sell prices slightly, this helps prevent silly 1-2% flips that might technically be optimal, but are rarely great in practice
# Do not try to sell and item if we can't sell for more than x gold a day, higher means more speed but less flexibility
min_move_per_day = 10 * gold
min_velocity = 5  # Do not try to buy/sell an item if we can get less than this amount of it in the alotted time, higher means more speed but less flexibility
click_weight = 20  # Up this if you waint more lines but larger chunks

solveroptions = {"threads": 8, "fracGap": 0.1, "maxSeconds": 240} # Options for the cbc solver

# put your own api key here obvs
apikey = '1070D853-612C-6042-AB29-69C9E2D06ACE7FE1C8F2-7AEE-4C3C-9646-F65BEC5E1F13'
