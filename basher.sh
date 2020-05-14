UNITY_VERSION=2019.2.5f1
docker run -it --rm \
-e "UNITY_USERNAME=kushal-shrestha@hotmail.com" \
-e "UNITY_PASSWORD=Kushal9841508775" \
-e "TEST_PLATFORM=linux" \
-e "WORKDIR=/root/project" \
-v "$(pwd):/root/project" \
gableroux/unity3d:$UNITY_VERSION \
bash