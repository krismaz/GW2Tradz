from pulp import LpVariable, LpProblem, value, LpMaximize, lpSum, LpStatus, COIN_CMD
from collections import defaultdict
from time import time
import utils
import options


def solve(operations, budget, simplicity):
    start = time()
    if simplicity <= 0:
        for operation in operations:
            operation.limiter = False
    
    operations = [op for op in operations if op.limit > 0]
    
    outputs = set()
    inputs = set()
    for operation in operations:
        outputs = outputs.union(operation.outputs.keys())
        inputs = inputs.union(operation.inputs.keys())
        

    operations = [op for op in operations if not set(op.inputs.keys()).difference(outputs)]
    operations = [op for op in operations if op.profit > 0 or set(op.outputs.keys()).intersection(inputs)]
    
    print(len(operations), 'Operations after filtering')

    lookup = defaultdict(list)
    for operation in operations:
        operation.lpvariable = LpVariable(
            operation.description, 0, operation.limit, cat='Integer')
        for id in operation.inputs.keys():
            lookup[id].append(operation)
        for id in operation.outputs.keys():
            lookup[id].append(operation)
        if operation.limiter:
            operation.indicator = LpVariable(
                operation.description + '_indicator', 0, operation.limit//operation.chunk_size + 1, cat='Integer')

    prob = LpProblem("SCIENCE!", LpMaximize)
    prob += lpSum([(op.profit - op.cost) *
                   op.lpvariable for op in operations]), "PROFIT!"

    for item, ops in lookup.items():
        prob += lpSum([(op.outputs.get(item, 0) - op.inputs.get(item, 0))
                       * op.lpvariable for op in ops]) >= 0

    for op in operations:
        #if op.cost > 0:
        #    prob += op.lpvariable * op.cost <= budget / simplicity * 3
        if op.indicator is not None:
            prob += op.lpvariable <= op.indicator * op.chunk_size
        #    prob += op.lpvariable >= op.indicator*0.1
        #    prob += op.lpvariable <= op.indicator*1e5
            

    prob += lpSum(op.cost * op.lpvariable for op in operations if op.cost) <= budget
    prob += lpSum(op.indicator for op in operations if op.indicator is not None) <= simplicity



    print('Pulp setup took', time() - start, 'seconds')
    print('Starting actual solve now!')
    start = time()

    solution = prob.solve(COIN_CMD('D:\\cbc\\bin\\cbc.exe', **options.solveroptions))


    print('Solution status:', LpStatus[solution],
          'in', time() - start, 'seconds')

    print(utils.coins(int(prob.objective.value())),' expected profit')

    for operation in operations:
        operation.value = value(operation.lpvariable)
