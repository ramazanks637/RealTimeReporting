#!/bin/sh
# PostgreSQL bağlantısı kurulana kadar bekler

until nc -z postgres 5432; do
  echo "PostgreSQL bekleniyor..."
  sleep 1
done

echo "PostgreSQL hazır!"
exec "$@"
