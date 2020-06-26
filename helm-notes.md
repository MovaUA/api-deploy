# questions:
- chart version semver 2 year.day-of-year.revision   yy.MM.dd-r
- chart appVersion
- chart apiVersion: v2
- helm lint
- separate build for helm chart?
- helm xxxx --verify: charts are not signed presently
- upgrade helm: see [supported versions](https://helm.sh/docs/topics/version_skew/#supported-version-skew)
- make sure labels and annotations are propagated
- YAML: two spaces (never tabs)

# chart structure:
- management service
  - mongodb
  - authorization.api
    - init: is mongodb ready?
    - mongodb.init (change-scripts are applied here)
    - mongodb.defaults (disscuss: should probably be a part of mongodb.init !?)
  - management.api
    - init: is authorization.api ready?
    - ingress resources
  - management.worker
    - init: is authorization.api ready?
