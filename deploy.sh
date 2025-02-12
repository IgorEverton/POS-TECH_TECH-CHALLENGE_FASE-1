#!/bin/bash

set -e  # Para encerrar o script em caso de erro

# 1. Construção das imagens
echo "🔨 Construindo imagens Docker..."
docker compose build

# 2. Removendo arquivos antigos para evitar conflitos
echo "🧹 Removendo manifests antigos..."
rm -rf k8s-manifests
mkdir k8s-manifests

# 3. Criando StorageClass "standard" se não existir
echo "🔧 Verificando StorageClass..."
if ! kubectl get storageclass standard &> /dev/null; then
  echo "📦 Criando StorageClass 'standard'..."
  cat <<EOF | kubectl apply -f -
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
  name: standard
provisioner: kubernetes.io/no-provisioner
volumeBindingMode: WaitForFirstConsumer
EOF
else
  echo "✅ StorageClass 'standard' já existe."
fi

# 4. Convertendo docker-compose para YAML do Kubernetes
echo "🔄 Convertendo docker-compose.yml para manifests Kubernetes..."
/mnt/c/Windows/System32/kompose.exe convert -f docker-compose.yml -o k8s-manifests/

# 5. Validando se os arquivos de Service foram gerados corretamente
echo "🔍 Verificando arquivos de Service..."
if ls k8s-manifests | grep -q "service"; then
  echo "✅ Services gerados corretamente."
else
  echo "⚠️ Nenhum Service gerado pelo Kompose! Verifique seu docker-compose.yml."
  exit 1
fi

# 6. Aplicando os recursos no Kubernetes
echo "📜 Aplicando recursos do Kubernetes..."
if [ -n "$(ls -A k8s-manifests 2>/dev/null)" ]; then
  kubectl apply -f k8s-manifests/
else
  echo "⚠️ Nenhum manifesto foi gerado! Verifique seu docker-compose."
  exit 1
fi

echo "✅ Deploy concluído!"
