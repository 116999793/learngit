#!/usr/bin/env python
# -*- coding: utf-8 -*-
print "aaaaaaaa"
print "bbbbbbbbbbbb"
print "cccc", "dddddd"
print 300
print 300+11
print "事实上+200=%d" % 100
#name = raw_input('enter your name')
#print "name =", name
print 0xff
print 1.23e-5
print r'''\\\n\'\t\""
 ...       adsfasdf
   afasdf
asdfasdfasfd'''
print not 3>2 or False
n = None
print n, "qq"
print 10/3
print 10.0/3
print chr(97)
classmate = ['aaa', 'bbb', 'ccc', 'dddd']
print len(classmate)
print classmate[-1]
classmate.append('eeeeee')
print classmate[-1]
classmate.pop(-1)
print classmate[-1]
classmate[-1] = 0xfffff
print classmate[-1]
classmate.append([11,22,33])
print classmate[-1][-1]
classmate2 = ('a1',)
print classmate2[-1]
i=3
if len(classmate) >i:
    print classmate[i-1]
else:
    print classmate[-1]
print 'OOOOKKKKK'

for clas in classmate:
    print  clas

print range(999)
#print int(raw_input('iippuutt:'))
d = {'01':'aa', '02':'bb', '03':'cc'}
print d['02']

def Myfunc(k, n='2', *args, **kw):
    return k+n+args+kw

#print Myfunc('=','ooo')

def calc(*num):
    sum = 0
    for n in num:
        sum=sum+n*n
    return sum

print calc(1,2,3,4)
print calc(*(1,2,3,4))
nums = [2,3,4,5]
print calc(*nums)

def perpon(name,age,**kw):
    print 'name:',name,'age:',age,'other:',kw
perpon('guokai',34)
ww={"www":35,"axy":60}
sorted(ww)
perpon('guokai',34,**ww)
b=1
L=[]
while b < 99:
    L.append(b)
    b+=2
print L[-2:]

arr = [x*x+y for x in range(1,11) if x%2==0 and x%4!=0 for y in range(1,3)]
print arr

import os
print [d for d in os.listdir('.')]
def feb(max):
    n,a,b=0,0,1
    while n<max:
        yield b
        a,b=b,a+b
        n+=1
print [x for x in feb(100)]
print [x for x in feb(100)]