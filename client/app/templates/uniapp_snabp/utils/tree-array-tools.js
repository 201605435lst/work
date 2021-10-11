/**
 * 为树状数组增加属性
 * @param { 原始数组 } treeArray
 * @param { 子节集合属性名 } childrenPropName
 * @param { 配置 } props [{sourceProp: 'name', targetProp: 'title', targetType: 'string', targetValue:null, handler:(item)=>{return any} }]
 */
export function treeArrayItemAddProps(treeArray, childrenPropName, props) {
	// 递归写法
	function loop(array) {
		for (const item of array) {
			for (const prop of props) {
				if (prop.sourceProp && prop.sourceProp.indexOf('{') > -1) {
					let sourceArray = prop.sourceProp.split('{');
					sourceArray.shift();

					sourceArray.map((sourceItem, index) => {
						// sourceItem = 'code} '  or 'name}'
						sourceItem = sourceItem.split('}')[0];

						const value = item[sourceItem].toString();
						item[prop.targetProp] =
							index > 0 ?
							item[prop.targetProp].replace('{' + sourceItem + '}', value) :
							prop.sourceProp.replace('{' + sourceItem + '}', value);
					});
				} else if (prop.handler) {
					item[prop.targetProp] = prop.handler(item);
				} else {
					if (item.hasOwnProperty(prop.sourceProp)) {
						const value = item[prop.sourceProp];
						item[prop.targetProp] =
							prop.targetType && prop.targetType === 'string' ? value.toString() : value;
					} else if (prop.targetProp) {
						item[prop.targetProp] = prop.targetValue;
					}
				}
			}
			if (item[childrenPropName]) {
				loop(item[childrenPropName]);
			}
		}
	}
	loop(treeArray);
	return treeArray;
}

/**
 * 把一个树状数组转换为数组
 * @param { 树状数组 } array
 */
export function treeArrayToFlatArray(treeArray, childrenProp = 'children') {
	let _array = [];

	function loop(tickArray) {
		for (let item of tickArray) {
			_array.push(item);
			let children = item[childrenProp];
			if (children && children.length > 0) {
				loop(children);
			}
		}
	}
	loop(treeArray);

	return _array;
}
