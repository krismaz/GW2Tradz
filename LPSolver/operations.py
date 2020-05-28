import options
import utils


class Operation:
    def __init__(self, cost, profit, inputs, outputs, limit, description, limiter=False, chunk_size=options.sanity):
        self.cost = cost
        self.profit = profit
        self.inputs = inputs
        self.outputs = outputs
        self.limit = limit
        self.description = description
        self.limiter = limiter
        self.chunk_size = chunk_size

        self.lpvariable = None
        self.value = None
        self.indicator = None



def FlipBuy(items):
    results = []

    for item in items:
        # Sanity
        if 'buy_price' not in item or item['buy_price'] == 0:
            continue

        # Reduce variance a bit
        if item['adjusted_buy'] < options.min_velocity:
            continue

        results.append(Operation(
            (item['buy_price'] + 1),
            0,
            {},
            {item['id']: 1},
            min(options.sanity, item['adjusted_buy']),
            f'Buy {item["name"]} ({item["id"]}) @ {utils.coins(item["buy_price"] + 1)}',
            True,
            chunk_size=250 * options.click_weight
        ))

    return results


def FlipSell(items):
    results = []
    for item in items:
        # Sanity
        if 'sell_price' not in item:
            continue

        # Reduce variance a bit
        if item['adjusted_sell'] < options.min_velocity:
            continue

        # save some computation speed
        if item['adjusted_sell'] * item['sell_price'] < options.min_move_per_day:
            continue

        results.append(Operation(
            0,
            (item['sell_price'] - 1)*0.85*options.safetyprice,
            {item['id']: 1},
            {},
            min(options.sanity, item['adjusted_sell']),
            f'Sell {item["name"]} ({item["id"]}) @ {utils.coins(item["sell_price"] - 1)}',
            False,
            chunk_size=250 * options.click_weight
        ))

    return results


def SpecialCrafting(recipes, names):
    daily = ['-260']
    results = []
    for recipe in recipes:
        # Remove Amalgamated Spam
        if recipe['name'] == "Amalgamated Gemstone":
            continue
        op = Operation(
            0,
            0,
            {i['item_id']: i['count'] for i in recipe['ingredients']},
            {recipe['output_item_id']: recipe['output_item_count']},
            1 if recipe['id'] in daily else options.sanity,
            f'Craft {recipe["name"]} from {", ".join(names.get(i["item_id"], "???") for i in recipe["ingredients"])} ({recipe["id"]})',
            False,
            chunk_size=1000 * options.click_weight
        )
        # Gold is handled as id -1
        if -1 in op.inputs:
            op.cost = op.inputs[-1]
            op.description = f'Buy {recipe["name"]} from vendor ({recipe["id"]})'
            op.chunk_size = options.sanity
            op.limiter = True
            del op.inputs[-1]
        results.append(op)
    return results

def Crafting(recipes, names, account_recipes):
    daily = [66913, 79795, 66993, 66917, 66923, 67377, 79726,
             79817, 79790, 46744, 79763, 46742, 46740, 46745, 67015]
    account_recipes = set(account_recipes)
    results = []

    for recipe in recipes:

        # skip duplicate mithrilium recipe
        if recipe['id'] == 12053:
            continue

        # skip unlearned recipes
        if recipe['id'] not in account_recipes and "LearnedFromItem" in recipe['flags']:
            continue

        results.append(Operation(
            0,
            0,
            {i['item_id']: i['count'] for i in recipe['ingredients']},
            {recipe['output_item_id']: recipe['output_item_count']},
            1 if recipe['output_item_id'] in daily else options.sanity,
            f'Craft {names.get(recipe["output_item_id"], "???")} from {", ".join(names.get(i["item_id"], "???") for i in recipe["ingredients"])} ({recipe["id"]})',
            recipe['type'] != 'Refinement',
            chunk_size=1000 * options.click_weight
        ))
    return results

def EctoSalvage():
        return [ Operation(
            60,
            0,
            {19721:1},
            {24277:1.85},
            options.sanity,
            f'Salvage Ecto',
            False
        )]

def Gemstones(names):
        stones = [ 24773, 24502, 24884, 24516, 24508, 24522, 72504, 70957, 72315, 76179, 74988, 24515, 75654, 24510, 24512, 76491, 24520, 42010, 72436, 24524, 24533, 24532, 24518, 24514 ]
        return [ Operation(
            0,
            0,
            {19721:5, stone:75},
            {68063:11.5},
            options.sanity,
            f'Make gemstones from ecto and {names[stone]}',
            True,
            250*options.click_weight
        ) for stone in stones]