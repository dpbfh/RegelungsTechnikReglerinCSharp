#!/usr/bin/env python3
# -*- coding: utf-8 -*-
#
#   FILE:
#       /atlantis/p/python/python4inf/pi-controller.py
#
#   SYNOPSIS:
#       Sprungantwort PI-Regler und PT1-Element.
#
################################################################################################

 

import time

 

from matplotlib import pyplot as plt

 


SignalValuesU = []

 


class PI:

 

    def __init__( self, KR, TR, Ts):
        self.__KR = KR
        self.__TR = TR
        self.__Ts = Ts

 

        self.__ek = [ 0] * 2
        self.__uk = [ 0] * 2
        return

 

    def read_from_hardware( self):
        self.__ek[ 0] = 1
                                        # Sprung, weil gesucht ist die Sprungantwort
        return

 

    def process( self):
        self.__uk[ 0] = self.__uk[ -1] + self.__KR * ((self.__TR + self.__Ts) * self.__ek[ 0] - self.__TR * self.__ek[ -1])

 

        self.__uk[ -1] = self.__uk[ 0]
        self.__ek[ -1] = self.__ek[ 0]
        return

 

    def write_to_hardware( self):
        SignalValuesU.append( self.__uk[ 0])
        return

 


if __name__ == "__main__":

 

    Ts = 0.010
    KR = 5
    TR = 1

 

    controller = PI( KR=KR, TR=TR, Ts=Ts)

 

    sim_time = 5
    sim_time_start = time.time()
    while time.time() - sim_time_start <= sim_time:
        controller.read_from_hardware()
        controller.process()
        controller.write_to_hardware()

 

        time.sleep( Ts)

 


    fig = plt.figure()
    axes = fig.subplots( 1, 2)
    tt = [ i * Ts for i in range( len( SignalValuesU))]
    ax = axes[ 0]
    ax.set_title( "Sprungantwort PI-Regler")
    ax.set_xlabel( "t")
    ax.set_ylabel( "u(t)")
    ax.plot( tt, SignalValuesU, "-r", label="u(t)...Stellgröße")
    ax.plot( tt, [ 1 for _ in range( len( SignalValuesU))], "-b", label="e(t)...Regeldifferenz")
    ax.legend()

 

    fig.show()
    plt.show()