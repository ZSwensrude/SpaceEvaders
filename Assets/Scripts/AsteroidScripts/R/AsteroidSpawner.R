args = commandArgs(trailingOnly = TRUE)

#GRAB RATE FROM SUPPLIED ARGUMENTS
lambda = as.numeric(args[1])
n = as.numeric(args[2])
interTimes = rexp(n, lambda)

interTimes