# Sys.sleep(5)
x <- matrix(1:10, ncol = 5)
print('from script 1')
pwd = getwd()
write(pwd, "")
write(x, paste(pwd, "/toto.out", sep = ""), sep = "\t", append = TRUE)
#write(x, paste(pwd, "/toto.out", sep = ""), sep = "\t")