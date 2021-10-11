// 获取 Cesium 比例尺
export function getCesiumDistanceLegend(scene, Cesium) {
  let distanceLegend = {
    width: undefined,
    distance: undefined,
    distanceRound: undefined,
    distanceRoundUnit: undefined,
  };

  let geodesic = new Cesium.EllipsoidGeodesic();
  let distances = [
    1,
    2,
    3,
    5,
    10,
    20,
    30,
    50,
    100,
    200,
    300,
    500,
    1000,
    2000,
    3000,
    5000,
    10000,
    20000,
    30000,
    50000,
    100000,
    200000,
    300000,
    500000,
    1000000,
    2000000,
    3000000,
    5000000,
    10000000,
    20000000,
    30000000,
    50000000,
  ];

  let width = scene.canvas.clientWidth;
  let height = scene.canvas.clientHeight;

  let left = scene.camera.getPickRay(new Cesium.Cartesian2((width / 2) | 0, height - 1));
  let right = scene.camera.getPickRay(new Cesium.Cartesian2((1 + width / 2) | 0, height - 1));

  let globe = scene.globe;
  let leftPosition = globe.pick(left, scene);
  let rightPosition = globe.pick(right, scene);

  if (!Cesium.defined(leftPosition) || !Cesium.defined(rightPosition)) {
    return;
  }

  let leftCartographic = globe.ellipsoid.cartesianToCartographic(leftPosition);
  let rightCartographic = globe.ellipsoid.cartesianToCartographic(rightPosition);

  geodesic.setEndPoints(leftCartographic, rightCartographic);
  let pixelDistance = geodesic.surfaceDistance;

  // Find the first distanceRound that makes the scale bar less than 100 pixels.
  let maxwidth = 100;
  let distanceRound;
  for (let i = distances.length - 1; !Cesium.defined(distanceRound) && i >= 0; --i) {
    if (distances[i] / pixelDistance < maxwidth) {
      distanceRound = distances[i];
    }
  }

  let distance = pixelDistance * 100;

  if (distance < 1) {
    distanceRound = 1;
  }

  if (Cesium.defined(distanceRound)) {
    if (distanceRound >= 1000) {
      distanceLegend.distanceRound = (distanceRound / 1000).toString();
      distance = distance / 1000;
      distanceLegend.distanceRoundUnit = ' km';
    } else {
      distanceLegend.distanceRound = distanceRound.toString();
      distanceLegend.distanceRoundUnit = ' m';
    }
    distanceLegend.distance = distance;
    distanceLegend.width = (distanceRound / pixelDistance) | 0;
  }

  return distanceLegend;
}

export function getCesiumPerformanceInfo() {
  // 获取 fps ms
  let $per = document.getElementsByClassName('cesium-performanceDisplay');
  let ms = null;
  let fps = null;
  if ($per && $per[0]) {
    ms = $per[0].children[0].innerText;
    ms = ms.replace('MS', '');
    fps = $per[0].children[1].innerText;
    fps = fps.replace('FPS', '');
    return { ms, fps };
  }
  return undefined;
}

/**
 * 相机漫游
 * @param {*} camera
 * @param {*} points
 * @param {*} callback  index：索引 finished 是否完成
 */
export function cameraFlyByPoints(camera, viewPoints, callback, speed = 1, startIndex = 0) {
  let index = startIndex;

  function loop() {
    if (index < viewPoints.length) {
      callback(index, false);
      let point = viewPoints[index];
      let prePoint = index > 0 ? viewPoints[index - 1] : null;
      let { position, orientation } = JSON.parse(JSON.stringify(point));
      let prePointObj = JSON.parse(JSON.stringify(prePoint));
      let duration = 1;

      if (index > 0) {
        let prePosition =
          startIndex != 0 && index == startIndex ? camera.position : prePointObj.position;

        duration =
          (Cesium.Cartesian3.distance(
            new Cesium.Cartesian3(prePosition.x, prePosition.y, prePosition.z),
            new Cesium.Cartesian3(position.x, position.y, position.z),
          ) *
            0.5) /
          speed;
      }

      // 定位 // 根据两点距离计算时长
      camera.flyTo({
        destination: new Cesium.Cartesian3(position.x, position.y, position.z),
        orientation,
        duration: Math.min(duration, 3),
        complete: data => {
          index++;
          loop();
        },
        easingFunction: time => time,
      });
    } else {
      if (index > 0 && index == viewPoints.length) {
        callback(null, true);
      }
    }
  }
  loop();
}

/**
 * 选中中心
 * @param {*} Cesium
 * @param {*} viewer
 */
export function registerMouseLock(Cesium, viewer) {
  let container = viewer.container;

  let $box = document.createElement('div');
  $box.style.width = 0;
  $box.style.height = 0;
  $box.style.top = '50%';
  $box.style.left = '50%';
  $box.style.display = 'none';
  $box.style.position = 'absolute';
  $box.style.alignItems = 'center';
  $box.style.justifyContent = 'center';
  $box.style.pointerEvents = 'none';

  $box.innerHTML = `<div style="width:50px; height:50px; min-width:50px; min-height:50px;"><svg id="图层_1" data-name="图层 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 43 43"><defs><style>.cls-1{fill:#fff;}.cls-2{opacity:0.3;isolation:isolate;}.cls-3{opacity:0.78;}</style></defs><title>UI</title><path class="cls-1" d="M1002.54,1377.43a13,13,0,1,0,13,13A13,13,0,0,0,1002.54,1377.43Zm0,24.5a11.5,11.5,0,1,1,11.5-11.5A11.5,11.5,0,0,1,1002.54,1401.93Z" transform="translate(-981.04 -1368.93)"/><path class="cls-2" d="M1002.54,1378.93a11.5,11.5,0,1,0,11.5,11.5A11.5,11.5,0,0,0,1002.54,1378.93Zm0,20.5a9,9,0,1,1,9-9A9,9,0,0,1,1002.54,1399.43Z" transform="translate(-981.04 -1368.93)"/><g class="cls-3"><path class="cls-1" d="M985.22,1388.43h-.07a.5.5,0,0,1-.43-.57,18.07,18.07,0,0,1,15.25-15.24.5.5,0,1,1,.14,1,17.07,17.07,0,0,0-14.4,14.4A.5.5,0,0,1,985.22,1388.43Z" transform="translate(-981.04 -1368.93)"/><path class="cls-1" d="M1019.86,1388.43a.5.5,0,0,1-.49-.43,17.06,17.06,0,0,0-14.4-14.4.51.51,0,0,1-.42-.57.49.49,0,0,1,.56-.42,18.07,18.07,0,0,1,15.25,15.24.5.5,0,0,1-.42.57Z" transform="translate(-981.04 -1368.93)"/><path class="cls-1" d="M1005,1408.25a.49.49,0,0,1-.49-.43.51.51,0,0,1,.42-.57,17.06,17.06,0,0,0,14.4-14.4.51.51,0,0,1,.57-.42.5.5,0,0,1,.42.57,18.1,18.1,0,0,1-15.25,15.25Z" transform="translate(-981.04 -1368.93)"/><path class="cls-1" d="M1000,1408.25H1000A18.1,18.1,0,0,1,984.72,1393a.5.5,0,0,1,1-.15,17.07,17.07,0,0,0,14.4,14.4.5.5,0,0,1-.07,1Z" transform="translate(-981.04 -1368.93)"/></g><path class="cls-1" d="M1002.54,1377.93a.5.5,0,0,1-.5-.5v-8a.5.5,0,0,1,1,0v8A.5.5,0,0,1,1002.54,1377.93Z" transform="translate(-981.04 -1368.93)"/><path class="cls-1" d="M1002.54,1411.93a.5.5,0,0,1-.5-.5v-8a.5.5,0,0,1,1,0v8A.5.5,0,0,1,1002.54,1411.93Z" transform="translate(-981.04 -1368.93)"/><path class="cls-1" d="M1023.54,1390.93h-8a.5.5,0,0,1-.5-.5.5.5,0,0,1,.5-.5h8a.5.5,0,0,1,.5.5A.5.5,0,0,1,1023.54,1390.93Z" transform="translate(-981.04 -1368.93)"/><path class="cls-1" d="M989.54,1390.93h-8a.5.5,0,0,1-.5-.5.5.5,0,0,1,.5-.5h8a.5.5,0,0,1,.5.5A.5.5,0,0,1,989.54,1390.93Z" transform="translate(-981.04 -1368.93)"/></svg></div>`;

  container.appendChild($box);

  let handler = new Cesium.ScreenSpaceEventHandler(viewer.scene.canvas);
  handler.setInputAction(event => {
    let position = event.position;
    $box.style.display = 'flex';
    $box.style.top = position.y + 'px';
    $box.style.left = position.x + 'px';
  }, Cesium.ScreenSpaceEventType.MIDDLE_DOWN);

  handler.setInputAction(event => {
    $box.style.display = 'none';
  }, Cesium.ScreenSpaceEventType.MIDDLE_UP);
}
