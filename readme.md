```
docker run --rm -v /dev/shm:/dev/shm --privileged -p 6080:26080 -e NOVNC=true       vivasaayi/selenium-with-nodejs:latest
```

```
for ((k=1;k<=50;k++)); 
do
  for ((i=1;i<=60;i++)); 
  do
    echo $i $k
    docker run --rm -d -v /dev/shm:/dev/shm --privileged -e NOVNC=true       vivasaayi/selenium-with-nodejs:latest &
  done
  sleep 45
done
```