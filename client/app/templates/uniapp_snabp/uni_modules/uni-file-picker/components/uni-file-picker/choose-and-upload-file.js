var Fly = require("flyio/dist/npm/wx")
var fly = new Fly; //创建fly实例

import {
  base64ToBlob,
  blobToBase64
} from 'base64-blob'


Object.defineProperty(exports, '__esModule', {
  value: true
});

const ERR_MSG_OK = 'chooseAndUploadFile:ok';
const ERR_MSG_FAIL = 'chooseAndUploadFile:fail';

function chooseImage(opts) {
  const {
    count,
    sizeType,
    sourceType,
    extension
  } = opts;
  return new Promise((resolve, reject) => {
    uni.chooseImage({
      count,
      sizeType,
      sourceType,
      extension,
      success(res) {
        resolve(normalizeChooseAndUploadFileRes(res, 'image'));
      },
      fail(res) {
        reject({
          errMsg: res.errMsg.replace('chooseImage:fail', ERR_MSG_FAIL),
        });
      },
    });
  });
}

function chooseVideo(opts) {
  const {
    camera,
    compressed,
    maxDuration,
    sourceType,
    extension
  } = opts;
  return new Promise((resolve, reject) => {
    uni.chooseVideo({
      camera,
      compressed,
      maxDuration,
      sourceType,
      extension,
      success(res) {
        const {
          tempFilePath,
          duration,
          size,
          height,
          width
        } = res;
        resolve(normalizeChooseAndUploadFileRes({
          errMsg: 'chooseVideo:ok',
          tempFilePaths: [tempFilePath],
          tempFiles: [{
            name: (res.tempFile && res.tempFile.name) || '',
            path: tempFilePath,
            size,
            type: (res.tempFile && res.tempFile.type) || '',
            width,
            height,
            duration,
            fileType: 'video',
            cloudPath: '',
          }, ],
        }, 'video'));
      },
      fail(res) {
        reject({
          errMsg: res.errMsg.replace('chooseVideo:fail', ERR_MSG_FAIL),
        });
      },
    });
  });
}

function chooseAll(opts) {
  const {
    count,
    extension
  } = opts;
  return new Promise((resolve, reject) => {
    let chooseFile = uni.chooseFile;
    if (typeof wx !== 'undefined' &&
      typeof wx.chooseMessageFile === 'function') {
      chooseFile = wx.chooseMessageFile;
    }
    if (typeof chooseFile !== 'function') {
      return reject({
        errMsg: ERR_MSG_FAIL + ' 请指定 type 类型，该平台仅支持选择 image 或 video。',
      });
    }
    chooseFile({
      type: 'all',
      count,
      extension,
      success(res) {
        resolve(normalizeChooseAndUploadFileRes(res));
      },
      fail(res) {
        reject({
          errMsg: res.errMsg.replace('chooseFile:fail', ERR_MSG_FAIL),
        });
      },
    });
  });
}

//BlobUrl转blob数据  
function objectURLToBlob(url, callback) {
  // var http = new XMLHttpRequest();
  // http.open("GET", url, true);
  // http.responseType = "blob";
  // http.onload = function(e) {
  //   if (this.status == 200 || this.status === 0) {
  //     callback(this.response)
  //   }
  // };
  // http.send()

  console.log('--------------objectURLToBlob')
  console.log(url)
  var p = plus.io.convertLocalFileSystemURL(url);
  console.log(p)
  // var locl = plus.io.LocalURL(p)
  console.log(plus.io)
  plus.io.resolveLocalFileSystemURL(p, function(entry) {
    console.log('+++++++++++++++++++++++++')
    console.log(entry)
    // 可通过entry对象操作test.html文件 
    entry.file(function(file) {
      console.log(file)
      reader = new plus.io.FileReader();
      reader.onloadend = function(e) {
        console.log("Read success");
        // Get data
        let resultBase64 = e.target.result
        console.log(resultBase64);
        base64ToBlob(resultBase64).then((res) => {
          console.log('----------------')
          console.log(res)
        })
      };
      reader.readAsDataURL(file);
    });
  }, function(e) {
    console.log(e)
  });



  // fly.get("https://baidu.com", null, {
  //   responseType: "blob"
  // }).then(d => {
  //   console.log(d)
  //   callback(d.data)
  //   //d.data 为ArrayBuffer实例
  // })
};
// //blob数据转file  
// objectURLToBlob('BlobUrl', function (blob) {  
//     let files = new window.File([blob], 'file.name', {type: 'file.type'})  
// })

function normalizeChooseAndUploadFileRes(res, fileType) {
  console.log('ddddddddddd')
  console.log(res)
  console.log(fileType)
  objectURLToBlob(res.tempFilePaths[0], (res) => {
    console.log(res)
  })

  // res.tempFiles.forEach((item, index) => {
  //   if (!item.name) {
  //     item.name = item.path.substring(item.path.lastIndexOf('/') + 1);
  //   }
  //   if (fileType) {
  //     item.fileType = fileType;
  //   }
  //   item.cloudPath =
  //     Date.now() + '_' + index + item.name.substring(item.name.lastIndexOf('.'));
  // });
  // // wx.chooseMessageFile
  // if (!res.tempFilePaths) {
  //   res.tempFilePaths = res.tempFiles.map((file) => file.path);
  // }
  // return res;
}

function uploadCloudFiles(res, max = 5, onUploadProgress) {
  res = Object.assign({}, res);
  res.errMsg = ERR_MSG_OK;
  const files = res.tempFiles;
  const len = files.length;
  let count = 0;
  return new Promise((resolve) => {
    while (count < max) {
      next();
    }

    function next() {
      let cur = count++;
      if (cur >= len) {
        !files.find((item) => !item.url && !item.errMsg) && resolve(res);
        return;
      }
      const fileItem = files[cur];
      uniCloud
        .uploadFile({
          filePath: fileItem.path,
          cloudPath: fileItem.cloudPath,
          fileType: fileItem.fileType,
          onUploadProgress(res) {
            res.index = cur;
            res.tempFile = fileItem;
            res.tempFilePath = fileItem.path;
            onUploadProgress &&
              onUploadProgress(res);
          },
        })
        .then((res) => {
          fileItem.url = res.fileID;
          if (cur < len) {
            next();
          }
        })
        .catch((res) => {
          // fileItem.errMsg = res.message;
          fileItem.errMsg = res.errMsg || res.message;
          if (cur < len) {
            next();
          }
        });
    }
  });
}

function uploadFiles(choosePromise, {
  onChooseFile,
  onUploadProgress
}) {
  return choosePromise
    .then((res) => {
      if (onChooseFile) {
        const customChooseRes = onChooseFile(res);
        if (typeof customChooseRes !== 'undefined') {
          return Promise.resolve(customChooseRes).then((chooseRes) => typeof chooseRes === 'undefined' ? res :
            chooseRes);
        }
      }
      return res;
    })
    .then((res) => {
      if (res === false) {
        return {
          errMsg: ERR_MSG_OK,
          tempFilePaths: [],
          tempFiles: [],
        };
      }
      return uploadCloudFiles(res, 5, onUploadProgress);
    });
}

function chooseAndUploadFile(opts = {
  type: 'all'
}) {
  if (opts.type === 'image') {
    return uploadFiles(chooseImage(opts), opts);
  } else if (opts.type === 'video') {
    return uploadFiles(chooseVideo(opts), opts);
  }
  return uploadFiles(chooseAll(opts), opts);
}

exports.chooseAndUploadFile = chooseAndUploadFile;
