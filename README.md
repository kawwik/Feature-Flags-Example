# Пример реализации фича-флагов на платформе Kubernetes

Использует в своей основе связку библиотеки Microsoft.FeatureManagement и ConfigMap.

## Структура репозитория

- Папка [ConfigMap](tree/master/ConfigMap) содержит Helm Chart для деплоя ConfigMap
и файлы с определениями флагов для Development и Production сред.
- Папка [Deploy](tree/master/Deploy) содержит манифест для размещения приложения
- Папка [ExampleApi](tree/master/ExampleApi) содержит тестовое API

## Описание примера

Тестовое API содержит примитивный метод для авторизации, принимающий логин и пароль.
В случае, если переданы верные данные, метод возвращает авторизационный токен.

Фича-флаг "CheckPassword" позволяет динамически выключать и включать проверку пароля.
Таким образом при значении флага false при вводе неправильного пароля всё равно будет возвращён токен.

В приложении содержится только один аккаунт с данными login:pass.

## Локальный запуск

Чтобы локально поднять Kubernetes можно воспользоваться инструментом [Minikube](https://kubernetes.io/ru/docs/tasks/tools/install-minikube/).

Сначала требуется разместить ConfigMap. Для этого скопируем нужный файл
с настройками в папку с чартом и создадим релиз:

```shell
cp .\ConfigMap\FeatureSettings\Development.json .\ConfigMap\Helm\feature-flags\
```

```shell
helm install feature-flags .\ConfigMap\Helm\feature-flags\features.json
```

Далее разместим само приложение:

```shell
kubectl apply -f .\Deploy\Application.yaml
```

Чтобы можно было сходить в API внутри контейнера, прокинем порты:

```shell
kubectl port-forward example-api 4000:4000
```

Теперь всё готово к использованию. Можем авторизоваться, используя curl:

```shell
curl --request POST -H "Content-Type: application/json" --data "{\"login\":\"login\", \"password\":\"pass\"}" http://localhost:4000/featureflagexample/login
```

Для переключения флага изменим значение в файле
features.json и обновим наш Helm релиз:

```shell
helm upgrade feature-flags .\ConfigMap\Helm\feature-flags\features.json
```