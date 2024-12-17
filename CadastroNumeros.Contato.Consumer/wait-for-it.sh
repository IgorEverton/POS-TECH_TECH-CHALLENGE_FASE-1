#!/usr/bin/env bash
set -e

host="$1"
shift
cmd="$@"

until nc -z "$host" 5672; do
  echo "Aguardando RabbitMQ em $host..."
  sleep 3
done

exec $cmd
