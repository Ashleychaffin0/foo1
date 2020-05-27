# Note: Uncomment the print statements below for debugging info

import math
import time

def PrintRange(r):
    sep = ''
    for n in r:
        print(sep, n, end='')
        sep = ", "
    print()

    # Found 78498 primesfrom 1 to 1000000 in 6.772272109985352
    # Enter prime count limit (0 to quit): 1,000,000
    # Count (78,498) by CountByDivision       = 78,498 in 00:00:00.1945348
    # Enter prime count limit (0 to quit):
    # Count (78,498) by CountByDivision       = 78,498 in 00:00:00.1259348


def CountByDivision(limit):
    numPrimesSoFar = 1
    for PotentialPrime in range(3, limit + 1, 2):
        # print("\nChecking", PotentialPrime)
        DivLimit = (int)(math.sqrt(PotentialPrime))
        # print("DivLimit = ", DivLimit)
        # r = range(3, DivLimit, 2)
        # print("\tChecking divisors ", end='')
        # PrintRange(r)
        bGotDivisor = False
        for Divisor in range(3, DivLimit + 1, 2):
            bGotDivisor = (PotentialPrime % Divisor) == 0
            # print(PotentialPrime, Divisor, bGotDivisor)
            if bGotDivisor:
                break
        if not bGotDivisor:
            # print("*********** Got a prime: ", PotentialPrime)
            numPrimesSoFar += 1    # Duh! Python doesn't have ++
    return numPrimesSoFar

start_time = time.time()
limit = 1000000
n = CountByDivision(limit)
elapsed_time = time.time() - start_time
print("Found", n, "primesfrom 1 to", limit, "in", elapsed_time)


































































