# Note: Uncomment the print statements below for debugging info

import math
import time

def PrintRange(r):
    sep = ''
    for n in r:
        print(sep, n, end='')
        sep = ", "
    print()

def CountByDivision(num):
    numPrimesSoFar = 1
    for PotentialPrime in range(3, num + 1, 2):
        # print("\nChecking", PotentialPrime)
        DivLimit = (int)(math.sqrt(PotentialPrime)) + 1
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
n = CountByDivision(1000000)
elapsed_time = time.time() - start_time
print(n, " primes in ", elapsed_time)


































































