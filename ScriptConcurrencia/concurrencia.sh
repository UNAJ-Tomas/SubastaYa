#!/bin/bash

URL="http://localhost:5288/api/Subastas/6/pujas"

BODY='{
  "subastaId": 6,
  "compradorId": 1,
  "monto": 110
}'

echo "Sending two identical bids concurrently..."
echo

curl -s -o response1.txt -w "%{http_code}" \
  -X POST "$URL" \
  -H "Content-Type: application/json" \
  -d "$BODY" > status1.txt &

curl -s -o response2.txt -w "%{http_code}" \
  -X POST "$URL" \
  -H "Content-Type: application/json" \
  -d "$BODY" > status2.txt &

wait

echo "Request 1: $(cat status1.txt)"
echo "Request 2: $(cat status2.txt)"

echo
echo "Response 1:"
cat response1.txt

echo
echo "Response 2:"
cat response2.txt