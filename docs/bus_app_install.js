var busApp = {
	'status':false,
	'setting':{
		upload:{
			"server":"https://buslikdrev.github.io/BusEngine/",
			"version":"1.0.13.3",
			"api":"module/bus_app",
			"lang":1,
			"offline_link":"offline.html",
			"offline_links":{
				"1":"offline.html"
			},
			"cache_status":true,
			"cache_resources":[],
			"cache_resources_exception":"https://translate.googleapis.com/,https://translate.google.com/,https://www.gstatic.com/",
			"cache_max_ages":604800,
			"cache_token":8,
			"push_status":false,
			"push_service":false,
			"sync_status":false,
			"debug":false
		},
		server:self.location.origin + '/',
		version:'1.0.0',
		api:'extension/module/bus_app',
		lang:1,
		offline_link:'/',
		offline_links:[],
		cache_status:false,
		cache_resources:[],
		cache_max_ages:604800,
		cache_token:1,
		push_status:false,
		sync_status:false,
		debug:false
	},
	'browser':function() {
		if ('userAgentData' in self.navigator) {
			return {name:self.navigator.userAgentData.brands['brand'], version:self.navigator.userAgentData.brands['version']};
		} else {
			var userAgent = self.navigator.userAgent.toLowerCase();
			//userAgent = 'Mozilla/5.0 (iPad; CPU OS 14_8 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Mobile/15E148 Safari/604.1'.toLowerCase();
			//userAgent = 'Mozilla/5.0 (iPhone; CPU iPhone OS 14_8 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/94.0.4606.76 Mobile/15E148 Safari/604.1'.toLowerCase();
			//userAgent = 'Mozilla/5.0 (Macintosh; Intel Mac OS X 11_1_0) AppleWebKit/536.30.1 (KHTML, like Gecko) Version/6.0.5 Safari/536.30.1'.toLowerCase();
			var browsers = userAgent.match(/(firefox|chrome|brave|vivaldi|edge|msie|opera|yabrowser|huaweibrowser|miuibrowser|safari)\/(\d+\.)/);
			if (browsers && browsers[1] == 'chrome' && browsers[2] >= '40.0') {
				var browsers2 = userAgent.match(/(brave|vivaldi|edge|msie|opera|yabrowser|huaweibrowser|miuibrowser)\/(\d+\.)/);
				if (browsers2) {
					browsers = browsers2;
				}
			}
			if (!browsers) {
				browsers = ['','',1000];
			}
			if (busApp.setting['debug'] > 0) {
				console.log(browsers)
			}

			return {name:browsers[1], version:parseFloat(browsers[2])};
		}
	},
	'cache': {
		'add':function(name, url, update) {
			if (url.indexOf('https') == -1) {
				return 'blocked http';
			}
			//var new_url = url.split('?');
			//url = new_url[0];
			if (typeof update === 'undefined') {
				var update = false;
			}
			// нужно доработать по параметрам
			self.caches.open(name).then(function(cache) {
				if (update) {
					cache.delete(url);

					//return cache.add(url);
					self.fetch(url).then(function(response) {
						return cache.put(url, response);
					});
				} else {
					cache.keys().then(function(cacheUrls) {
						if (cacheUrls.length) {
							for (var i = 0; i < cacheUrls.length; i++) {
								if (cacheUrls[i].url.indexOf(url) != -1) {
									return false;
								}
							}
						}

						//return cache.add(url);
						self.fetch(url).then(function(response) {
							return cache.put(url, response);
						});
					});
				}
			});
		},
		'addAll':function(name, urls) {
			var new_urls = [];
			var ii = 0
			for (var i = 0; i < urls.length; i++) {
				if (new_urls.indexOf(urls[i]) == -1) {
					//var new_url = urls[i].split('?');
					//urls[i] = new_url[0];
					if (urls[i].indexOf('https') != -1) {
						new_urls[ii++] = urls[i];
					}
				}
			}
			caches.open(name).then(function(cache) {
				cache.addAll(new_urls);
			});
		},
	},
	'ajax':function(url, setting) {
		if (typeof setting['metod'] === 'undefined') {
			setting['metod'] = 'GET';
		}
		if (typeof setting['responseType'] === 'undefined') {
			setting['responseType'] = 'json';
		}
		if (typeof setting['dataType'] === 'undefined') {
			setting['dataType'] = 'text';
		}
		if (typeof setting['data'] === 'undefined') {
			setting['data'] = '';
		}
		if (typeof setting['async'] === 'undefined') {
			setting['async'] = true;
		}
		if (typeof setting['user'] === 'undefined') {
			setting['user'] = null;
		}
		if (typeof setting['password'] === 'undefined') {
			setting['password'] = null;
		}
		if (typeof setting['success'] === 'undefined') {
			setting['success'] = function(json) {};
		}
		if (typeof setting['error'] === 'undefined') {
			setting['error'] = function(error) {};
		}
		var datanew = null;
		if (setting['data']) {
			if (setting['dataType'] == 'json') {
				datanew = JSON.stringify(setting['data']);
			} else {
				if (typeof FormData !== 'undefined') {
					datanew = new FormData();
					if (typeof setting['data'] == 'object') {
						for (var i in setting['data']) {
							if (typeof setting['data'][i] == 'object') {
								for (var i2 in setting['data'][i]) {
									if (typeof setting['data'][i][i2] == 'object') {
										for (var i3 in setting['data'][i][i2]) {
											datanew.append(i + '[' + i2 + ']' + '[' + i3 + ']', setting['data'][i][i2][i3]);
										}
									} else {
										datanew.append(i + '[' + i2 + ']', setting['data'][i][i2]);
									}
								}
							} else {
								datanew.append(i, setting['data'][i]);
							}
						}
					} else {
						datanew = setting['data'];
					}
				} else {
					datanew = [];
					if (typeof setting['data'] == 'object') {
						for (var i in setting['data']) {
							if (typeof setting['data'][i] == 'object') {
								for (var i2 in setting['data'][i]) {
									if (typeof setting['data'][i][i2] == 'object') {
										for (var i3 in setting['data'][i][i2]) {
											datanew.push(encodeURIComponent(i) + '[' + encodeURIComponent(i2) + ']' + '[' + encodeURIComponent(i3) + ']=' + encodeURIComponent(setting['data'][i][i2][i3]));
										}
									} else {
										datanew.push(encodeURIComponent(i) + '[' + encodeURIComponent(i2) + ']=' + encodeURIComponent(setting['data'][i][i2]));
									}
								}
							} else {
								datanew.push(encodeURIComponent(i) + '=' + encodeURIComponent(setting['data'][i]));
							}
						}
					} else {
						datanew = setting['data'];
					}

					datanew = datanew.join('&').replace(/%20/g, '+');
				}
			}
		}

		var xhr = new XMLHttpRequest();
		xhr.open(setting['metod'], url, setting['async'], setting['user'], setting['password']);
		xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
		if (typeof FormData === 'undefined') {
			if (setting['dataType'] == 'json') {
				xhr.setRequestHeader('Content-type', 'application/json;charset=UTF-8');
			} else if (setting['dataType'] == 'text') {
				xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded; charset=UTF-8');
			}
		}
		if (setting['responseType']) {
			xhr.responseType = setting['responseType']; //"text" – строка,"arraybuffer", "blob", "document", "json" – JSON (парсится автоматически).
		}
		if (busApp.setting['debug'] == 1) {
			console.log('xhr data: ', datanew);
		}
		xhr.send(datanew);
		xhr.onload = function(oEvent) {
			if (xhr.status == 200) {
				return setting['success'](xhr.response, xhr);
			} else {
				var ajaxOptions = setting;
				var thrownError = false;
				return setting['error'](xhr, ajaxOptions, thrownError);
			}
		};

		//return xhr;
	}
};

if (busApp.setting['upload'] != 'SETTING_DATA') {
	busApp.setting = busApp.setting['upload'];
	busApp.setting['browser'] = busApp.browser();
}

self.addEventListener('message', function(event) {
	if (typeof event.data['lang'] != 'undefined') {
		busApp.setting['lang'] = event.data['lang'];
		if (busApp.setting['offline_links'][event.data['lang']]) {
			busApp.setting['offline_link'] = busApp.setting['offline_links'][event.data['lang']];
		}
	}
	// event is an ExtendableMessageEvent object
	//console.log('The client sent me a message: ', event.data);
	//event.source.postMessage('Hi client');
});

self.addEventListener('install', function(event) {
	if (busApp.setting['debug']) {
		console.log('self install:', event);
	}
	//self.skipWaiting();
	event.waitUntil(self.skipWaiting());
	if (busApp.status == false) {
		busApp.status = true;
		event.waitUntil(
			caches.keys().then(function(keys) {
				for (var i in keys) {
					if (keys[i].indexOf('bus-app-' + busApp.setting['cache_token']) == -1) {
						caches.delete(keys[i]);
					} else if (1 == 0 && keys[i].indexOf('bus-app') != -1) {
						/* if (busApp.setting['debug']) {
							console.log('self caches1:', parseInt(keys[i].substring(8)));
							console.log('self caches2:', Math.round(Date.now() / 1000 + 62));
							console.log('self caches3:', (Math.round(Date.now() / 1000 + 62) + busApp.setting['cache_max_ages']));
						}
						if (parseInt(keys[i].substring(8)) < Math.round(Date.now() / 1000 + 100)) {
							caches.delete(keys[i]);
							busApp.setting['cache_token'] = (Math.round(Date.now() / 1000 + 100) + busApp.setting['cache_max_ages']);
						} */
					}
				}
			})
		)
		// статика
		for (var i in busApp.setting['cache_resources']) {
			busApp.cache.add('bus-app-' + busApp.setting['cache_token'], busApp.setting['cache_resources'][i], true);
		}
		//busApp.cache.addAll('bus-app-' + busApp.setting['cache_token'], busApp.setting['cache_resources']);
	}
});

self.addEventListener('activate', function(event) {
	if (busApp.setting['debug']) {
		console.log('self activate:', event);
	}
	//self.skipWaiting();
	event.waitUntil(self.skipWaiting());
	event.waitUntil(clients.claim());
	if (busApp.status == false) {
		busApp.status = true;
		event.waitUntil(
			caches.keys().then(function(keys) {
				for (var i in keys) {
					if (keys[i].indexOf('bus-app-' + busApp.setting['cache_token']) == -1) {
						caches.delete(keys[i]);
					} else if (1 == 0 && keys[i].indexOf('bus-app') != -1) {
						/* if (busApp.setting['debug']) {
							console.log('self caches1:', parseInt(keys[i].substring(8)));
							console.log('self caches2:', Math.round(Date.now() / 1000 + 62));
							console.log('self caches3:', (Math.round(Date.now() / 1000 + 62) + busApp.setting['cache_max_ages']));
						}
						if (parseInt(keys[i].substring(8)) < Math.round(Date.now() / 1000 + 100)) {
							caches.delete(keys[i]);
							busApp.setting['cache_token'] = (Math.round(Date.now() / 1000 + 100) + busApp.setting['cache_max_ages']);
						} */
					}
				}
			})
		)
		// статика
		for (var i in busApp.setting['cache_resources']) {
			busApp.cache.add('bus-app-' + busApp.setting['cache_token'], busApp.setting['cache_resources'][i], true);
		}
		//busApp.cache.addAll('bus-app-' + busApp.setting['cache_token'], busApp.setting['cache_resources']);
	}
});

self.addEventListener('wait', function(event) {
	//console.log('self wait:', event);
});

self.addEventListener('redundant', function(event) {
	//console.log('self redundant:', event);
});

//https://learn.javascript.ru/fetch-api#cache
//event.request.arrayBuffer()
//event.request.blob()
//event.request.json()
//event.request.text()
//event.request.formData()
self.addEventListener('fetch', function(event) {
	if (!busApp.setting['cache_status']) {
		if (self.navigator.onLine) {
			return event.request;
		} else {
			return self.caches.match(busApp.setting['offline_link']);
		}
	}
	if (event.request.url.indexOf('https') == -1) {
		console.log(event.request.url);
		return event.request;
	}
	// исключение из кэша
	var url_exception = new URL(event.request.url);
	if (url_exception.pathname && url_exception.pathname != '/' && busApp.setting['cache_resources_exception'].indexOf(url_exception.pathname) != -1 || url_exception.hostname && busApp.setting['cache_resources_exception'].indexOf(url_exception.hostname) != -1) {
		return event.request;
	}

	var request = event.request;

	if (request.referrerPolicy == 'unsafe-url' && request.mode == 'navigate') {
		console.log(request.url, request);
	}

	if (request.method == 'GET') {
		/* if (request.destination == 'image') {
			var new_url = request.url.split('?');
			request.url = new_url[0];
		} */

		event.respondWith(self.caches.open('bus-app-' + busApp.setting['cache_token']).then(function(cache) {
			return cache.match(request.url).then(function(cachesResponse) {
				// постоянное обновление кэша
				if (self.navigator.onLine && ((cachesResponse && 'headers' in cachesResponse && cachesResponse.headers.get('content-type') != null ? cachesResponse.headers.get('content-type').indexOf('text/html') != -1 : false) || cachesResponse && request.destination == 'document')) {
					return fetch(request).then(function(lanResponse) {
						return cache.put(request.url, lanResponse.clone()).then(function(sssss) {
							return lanResponse;
						});
					}).catch(function(error) {
						return cachesResponse;
					});
				// обычная работа кэша
				} else {
					if (cachesResponse) {
						return cachesResponse;
					} else {
						if (self.navigator.onLine) {
							return fetch(request).then(function(lanResponse) {
								return cache.put(request.url, lanResponse.clone()).then(function(sssss) {
									return lanResponse;
								});
							});
						} else {
							return cache.match(busApp.setting['offline_link']);
						}
					}
				}
			}).catch(function(error) {
				return cache.match(busApp.setting['offline_link']);
			});
		}));
	} else {
		return event.request;
	}
});

if (busApp.setting['push_status']) {
	self.addEventListener('push', function(event) {
		var status = false;

		if ('Notification' in self) {
			status = true;
			Notification = self.Notification;
		} else if ('mozNotification' in self) {
			Notification = self.mozNotification;
			status = true;
		} else if ('webkitNotifications' in self) {
			Notification = self.webkitNotifications;
			status = true;
		}

		if (!(status && Notification.permission === 'granted')) {
			return;
		}
		
		if (busApp.setting['debug'] == 1) {
			console.log('push: ', event);
		}

		if (event.data) {
			var data = event.data.json();
			if (busApp.setting['debug'] == 1) {
				console.log('push data: ', data);
			}
			if ('notification' in data) {
				if (!('data' in data.notification) && 'data' in data) {
					if ('gcm.notification.data' in data.data) {
						data.notification.data = JSON.parse(data.data['gcm.notification.data']);
					}
				}
				event.waitUntil(self.registration.showNotification(data.notification.title, data.notification));
			} else {
				if (busApp.setting['debug'] == 1) {
					console.log('push data test: ', event);
				}
				// Так как пока невозможно передавать данные от push-сервера,
				// то информацию для уведомлений получаем с нашего сервера
				event.waitUntil(
					self.registration.pushManager.getSubscription().then(function(subscription) {
						if (subscription) {
							busApp.ajax(busApp.setting['server'] + 'index.php?route=' + busApp.setting['api'] + '/push', {
								metod: 'POST',
								data: {api:JSON.parse(JSON.stringify(subscription)), get:true},
								success:function(json, xhr) {
									if (typeof json === 'object') {
										for (var i in json) {
											self.registration.showNotification(json[i][0]['title'], json[i][0]['option']);
										}
									}
								},
								error: function(xhr, ajaxOptions, thrownError) {
									alert('Error ' + xhr.status);

									console.error('Bus_app Push Невозможно получить данные с сервера: ', xhr);

									return self.registration.showNotification('Ошибочка вышла', {
										body: 'Мы хотели сообщить вам что-то важное, но у нас всё сломалось.',
										icon: 'image/catalog/bus_app/favicon-192x192.png',
										tag: 'notification-error',
										data: {url:busApp.setting['server']}, //the url which we gonna use later
										actions: [{action: "open_url", title: "Read Now"}]
									});
								}
							});
						}
					})
				);
			}
		}
	});

	self.addEventListener('notificationclick', function(event) {
		if (busApp.setting['debug'] == 1) {
			console.log('Пользователь кликнул по уведомлению: ', event.notification);
		}

		event.notification.close();

		// Смотрим, открыта ли вкладка с данной ссылкой
		// и фокусируемся или открываем ссылку в новой вкладке
		event.waitUntil(
			clients.matchAll({
				type: 'window'
			}).then(function(clientList) {
				console.log(event);
				if ('data' in event.notification) {
					var url = event.notification.data.url.replace(/&amp;/g, '&');

					for (var i = 0; i < clientList.length; i++) {
						var client = clientList[i];
						if (client.url == url && 'focus' in client) {
							return client.focus();
						}
					}

					if (clients.openWindow) {
						return clients.openWindow(url);
					}
				}
			})
		);
	});
}

if (busApp.setting['sync_status'] && 1 == 0) {
	console.log('online', self);
	self.addEventListener('online', function(event) {
		console.log('online', self);
	});
	self.addEventListener('sync', function(event) {
		if (event.tag == 'bus-app') {
			console.log('sync', event);
		}
		/* if (event.tag == 'bus-app22') {
			event.waitUntil(
				readAllData('bus-app').then(async function(data) {
					const requests = [];

					for (const d of data) {
						requests.push(fetch('https://simple-pwa-8a005.firebaseio.com/data.json', {
							method: 'POST',
							headers: {
								'Content-Type': 'application/json',
								'Accept': 'application/json'
							},
							body: JSON.stringify({
								sunday: d.sunday
							})
						}));
					}

					const results = await Promise.all(requests);

					results.map(function(response, index) {
						if (response.ok) {
							deleteItemFromData('bus-app', data[index].id);
						}
					})
				})
			);
		} */
	});
}