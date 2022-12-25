/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

var busApp = {
	'setting':{
		browser:{name:'', version:0},
		display:'',
		version:'1.0.0',
		api:'extension/module/bus_app',
		lg:true,
		md:true,
		sm:true,
		xs:true,
		lang:1,
		name:'',
		description:'',
		description_notification:'',
		offline:'OffLine - not internet!',
		offline_link:'offline.html',
		online:'OnLine - Yes internet!',
		appinstalled:'App installed!',
		delay:5000,
		closeTime:30000000,
		cache_status:false,
		cache_resources:[],
		cache_resources_exception:"",
		cache_max_ages:604800,
		cache_token:1,
		notification_status:false,
		notification_interval:3600,
		notification_error:true,
		push_status:false,
		sync_status:false,
		start_url:'/',
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
	'isButton':function() {
		var bus_app_id = document.getElementById('bus-app');
		if (bus_app_id && bus_app_id.querySelectorAll('[data-button]:not([hidden])').length) {
			return true;
		} else {
			return false;
		}
	},
	'status':false,
	'start':function(busAppSetting) {
		if (busApp.status == false) {
			busApp.status = true;
			document.removeEventListener('DOMContentLoaded', busApp.start, {once:true, passive:true});
			document.removeEventListener('visibilitychange', busApp.start, {once:true, passive:true});
			window.removeEventListener('load', busApp.start, {once:true, passive:true});
			window.removeEventListener('pagehide', busApp.start, {once:true, passive:true});
			window.removeEventListener('scroll', busApp.start, {once:true, passive:true});
			window.removeEventListener('mouseover', busApp.start, {once:true, passive:true});
			window.removeEventListener('touchstart', busApp.start, {once:true, passive:true});
		} else {
			console.log('Bus_app уже работает!');
			return true;
		}

		if (typeof busAppSetting !== 'undefined' && typeof busAppSetting === 'object') {
			var setting = {'name': 'name'};
			var setting2 = {'name': 'name2'};
			var i;
			for (i in setting) {
				if (i == 'name') {
					setting2['name'] = setting['name'];
				}
				setting2[i] = setting[i];
			}
		}

		/* if (typeof window.Event !== 'function') {
			window.Event = function(event, params) {
				params = params || {bubbles:false, cancelable:false, composed:null};

				var evt = document.createEvent('Event');  //"Event", "UIEvents", "MouseEvents", "MutationEvents", and "HTMLEvents"
				evt.initEvent(event, params.bubbles, params.cancelable);

				return evt;
			}
		} */
		if (typeof window.CustomEvent !== 'function') {
			window.CustomEvent = function(event, params) {
				params = params || {bubbles:false, cancelable:false, detail:null};

				var evt = document.createEvent('CustomEvent');
				evt.initCustomEvent(event, params.bubbles, params.cancelable, params.detail);

				return evt;
			};
		}

		var element = new CustomEvent('busAppBefore', {bubbles: true});
		document.dispatchEvent(element);

		var status = false;

		if ('matchMedia' in window) {
			if (busApp.setting['lg'] == true && window.matchMedia('(min-width: 1200px)').matches) {
				status = true;
			} else if (busApp.setting['md'] == true && window.matchMedia('(min-width: 992px) and (max-width:1199px)').matches) {
				status = true;
			} else if (busApp.setting['sm'] == true && window.matchMedia('(min-width: 768px) and (max-width:991px)').matches) {
				status = true;
			} else if (busApp.setting['xs'] == true && window.matchMedia('(max-width: 767px)').matches) {
				status = true;
			}
		} else {
			if (busApp.setting['lg'] == true && window.innerWidth >= 1200) {
				status = true;
			} else if (busApp.setting['md'] == true && window.innerWidth >= 992 && window.innerWidth <= 1199) {
				status = true;
			} else if (busApp.setting['sm'] == true && window.innerWidth >= 768 && window.innerWidth <= 991) {
				status = true;
			} else if (busApp.setting['xs'] == true && window.innerWidth < 768) {
				status = true;
			}
		}

		if (status == true) {
			busApp.setting['browser'] = busApp.browser();
			/* CSS */
			//@media all and (display-mode: standalone) {}
			if ('matchMedia' in window && window.matchMedia('(display-mode: browser)').matches) {
				busApp.setting['display'] = 'browser';
			} else if ('matchMedia' in window && window.matchMedia('(display-mode: minimal-ui)').matches) {
				busApp.setting['display'] = 'minimal-ui; ';
			} else if ('matchMedia' in window && window.matchMedia('(display-mode: standalone)').matches) {
				busApp.setting['display'] = 'standalone';
			} else if ('matchMedia' in window && window.matchMedia('(display-mode: fullscreen)').matches) {
				busApp.setting['display'] = 'fullscreen';
			} else {
				if (!window.localStorage.getItem('bus-app-block-height') || !('standalone' in window.navigator && navigator.standalone)) {
					busApp.setting['display'] = 'browser';
				} else {
					busApp.setting['display'] = 'standalone';
				}
			}
			var element = new CustomEvent('busApp', {bubbles: true});
			document.dispatchEvent(element);
			var debug = {};
			if (busApp.setting['debug']) {
				var start = new Date().getTime();
				debug['UserAgentJS'] = window.navigator.userAgent;
				debug['api'] = busApp.setting['api'];
				debug['Ширина экрана'] = window.innerWidth + 'px';
				debug['Высота экрана'] = window.innerHeight + 'px';
			}
			var bus_app_id = document.getElementById('bus-app');
			// настройки блока
			if (bus_app_id) {
				var beforeinstallprompt;
				var name = bus_app_id.querySelector('[data-message="name"]');
				if (name) {
					name.innerHTML = busApp.setting['name'];
				}
				var line = document.querySelector('#bus-app-line');

				if ('onbeforeinstallprompt' in window && !(busApp.setting['browser']['name'] == 'chrome' && busApp.setting['browser']['version'] >= '40.0' && busApp.setting['browser']['version'] < '66.0' || busApp.setting['browser']['name'] == 'huaweibrowser' || busApp.setting['browser']['name'] == 'miuibrowser')) {
					// fix запуска установки, если отложили загрузку скрипта
					if (busApp.setting['beforeinstallprompt']) {
						beforeinstallprompt = busApp.setting['beforeinstallprompt'];
						if (busApp.setting['display'] == 'browser' && !window.localStorage.getItem('bus-app-block') || busApp.setting['display'] == 'browser' && window.localStorage.getItem('bus-app-block') && (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime']) {
							var desc = bus_app_id.querySelector('[data-message="desc"]');
							if (desc) {
								desc.innerHTML += busApp.setting['description'] + ' ';
							}
							var button = bus_app_id.querySelector('[data-button="install"]');
							if (button) {
								button.removeAttribute('hidden');
							}
							setTimeout(function() {
								bus_app_id.removeAttribute('hidden');
							}, busApp.setting['delay']);
						}
					}
					// основная технология установки приложения
					window.addEventListener('beforeinstallprompt', function(event) {
						beforeinstallprompt = event;
						event.preventDefault();
						if (busApp.setting['display'] == 'browser' && !window.localStorage.getItem('bus-app-block') || busApp.setting['display'] == 'browser' && window.localStorage.getItem('bus-app-block') && (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime']) {
							var desc = bus_app_id.querySelector('[data-message="desc"]');
							if (desc) {
								desc.innerHTML += busApp.setting['description'] + ' ';
							}
							var button = bus_app_id.querySelector('[data-button="install"]');
							if (button) {
								button.removeAttribute('hidden');
							}
							setTimeout(function() {
								bus_app_id.removeAttribute('hidden');
							}, busApp.setting['delay']);
						}
					}, false);

					bus_app_id.querySelector('[data-button="install"]').addEventListener('click', function() {
						if (beforeinstallprompt) {
							if ('setAppBadge' in window.navigator) {
								window.navigator.setAppBadge(parseFloat(busApp.setting['version']));
							}
							busApp.close('bus-app');
							beforeinstallprompt.prompt();
							beforeinstallprompt.userChoice.then(function() {
								beforeinstallprompt = null;
							});
							if (busApp.setting['start_url']) {
								window.location.href = busApp.setting['start_url'];
							} else {
								window.location.href = window.location.href;
							}
						}
					});
				} else {
					// вариант установки приложения по умолчанию
					debug = busApp.install({'bus_app_id':bus_app_id, 'debug':debug});
				}

				// сообщение, что приложение установлено
				window.addEventListener('appinstalled', function(event) {
					if (line) {
						var desc = line.querySelector('[data-message]');
						if (desc) {
							desc.setAttribute('data-message', 'online');
							desc.innerHTML = busApp.setting['appinstalled'];
						}
						var box = line.querySelector('[data-box]');
						if (box) {
							box.style['min-height'] = '60px';
						}
						line.removeAttribute('hidden');
						setTimeout(function() {
							line.setAttribute('hidden', true);
						}, 2000);
						if (busApp.setting['debug']) {
							debug['appinstalled'] = busApp.setting['appinstalled'];
						}
					}
				}, false);

				// нет интернета
				window.addEventListener('offline', function() {
					if (!window.localStorage.getItem('bus-app-line-block') || window.localStorage.getItem('bus-app-line-block') && (Date.now() - window.localStorage.getItem('bus-app-line-block')) > busApp.setting['closeTime']) {
						if (line) {
							var desc = line.querySelector('[data-message]');
							if (desc) {
								desc.setAttribute('data-message', 'offline');
								desc.innerHTML = busApp.setting['offline'];
							}
							var box = line.querySelector('[data-box]')
							if (box) {
								box.style['min-height'] = '60px';
							}
							line.removeAttribute('hidden');
						}
					}

					window.alert = null;
				}, false);

				// есть интернет
				window.addEventListener('online', function() {
					if (line) {
						var desc = line.querySelector('[data-message]');
						if (desc) {
							desc.setAttribute('data-message', 'online');
							desc.innerHTML = busApp.setting['online'];
						}
						var box = line.querySelector('[data-box]')
						if (box) {
							box.style['min-height'] = '60px';
						}
						line.removeAttribute('hidden');
						setTimeout(function() {
							line.setAttribute('hidden', true);
						}, 2000);
					}
				}, false);
			}

			// кэшируем при первом посещении
			if (busApp.setting['cache_status'] && 'CacheStorage' in window && window.navigator.onLine) {
				// статика дополнительно
				window.caches.has('bus-app-' + busApp.setting['cache_token']).then(function(has) {
					if (!has) {
						var i;
						// стили
						var stylesheets = document.getElementsByTagName('link');
						if (stylesheets) {
							for (i in stylesheets) {
								if (stylesheets[i].href && stylesheets[i].href != window.location.href) {
									busApp.cache.add('bus-app-' + busApp.setting['cache_token'], stylesheets[i].href);
								}
							}
						}

						// скрипты
						for (i in document.scripts) {
							if (document.scripts[i].src) {
								busApp.cache.add('bus-app-' + busApp.setting['cache_token'], document.scripts[i].src);
							}
						}

						// изображения
						for (i in document.images) {
							if (document.images[i].src) {
								busApp.cache.add('bus-app-' + busApp.setting['cache_token'], document.images[i].src);
							}
						}

						// ссылки
						/* for (i in document.links) {
							if (document.links[i].href && isUrlDynamic(document.links[i].href)) {
								busApp.cache.add('bus-app-' + busApp.setting['cache_token'], document.links[i].href);
							}
						} */

						// свои ссылки
						for (i in busApp.setting['cache_resources']) {
							busApp.cache.add('bus-app-' + busApp.setting['cache_token'], busApp.setting['cache_resources'][i]);
						}

						// заглушка
						busApp.cache.add('bus-app-' + busApp.setting['cache_token'], busApp.setting['offline_link']);
						busApp.ajax(busApp.setting['offline_link'], {
							'responseType': 'document',
							'success': function(doc, xhr) {
								// стили
								var i, stylesheets = doc.getElementsByTagName('link');
								if (stylesheets) {
									for (i in stylesheets) {
										if (stylesheets[i].href && stylesheets[i].href != window.location.href) {
											busApp.cache.add('bus-app-' + busApp.setting['cache_token'], stylesheets[i].href);
										}
									}
								}

								// скрипты
								for (i in doc.scripts) {
									if (doc.scripts[i].src) {
										busApp.cache.add('bus-app-' + busApp.setting['cache_token'], doc.scripts[i].src);
									}
								}

								// изображения
								for (i in doc.images) {
									if (doc.images[i].src) {
										busApp.cache.add('bus-app-' + busApp.setting['cache_token'], doc.images[i].src);
									}
								}

								//busApp.cache.add('bus-app-' + busApp.setting['cache_token'], xhr.responseURL);
							}
						});
					}
				});

				// динамика
				busApp.cache.add('bus-app-' + busApp.setting['cache_token'], window.location.href, true);
			}

			// запуск сервис воркера
			if ('serviceWorker' in window.navigator) {
				if (window.navigator.onLine == false) {
					window.alert = null;
				}
				if (busApp.setting['debug']) {
					debug['serviceWorker'] = 'yes support';
				}

				window.navigator.serviceWorker.register('bus_app_install.js?v=' + busApp.setting['cache_token']/* , {
					scope: './BusEngine/',
					//updateViaCache: 'none', // imports, all, none
				} */).then(function (registration) {
					/* if (typeof registration.update === 'function') {
						console.log('Bus_app update ', typeof registration.update);
						registration.update();
					} */
					var serviceWorker;
					if (registration.installing) {
						serviceWorker = registration.installing;
						if (busApp.setting['debug'] == 1) {
							console.log('Bus_app installing:', registration);
						}

					} else if (registration.waiting) {
						serviceWorker = registration.waiting;
						if (busApp.setting['debug'] == 1) {
							console.log('Bus_app waiting:', registration);
						}
					} else if (registration.active) {
						serviceWorker = registration.active;
						if (busApp.setting['debug'] == 1) {
							console.log('Bus_app active:', registration);
						}
					}
					if (serviceWorker) {
						if (busApp.setting['debug'] == 1) {
							console.log('Bus_app state: ', serviceWorker);
						}

						serviceWorker.addEventListener('statechange', function(e) {
							if (1 == 1 || busApp.setting['debug'] == 1) {
								console.log('Bus_app statechange: ', e.target.state);
							}

							if (e.target.state == 'activated') {

							}
						});

						// Notification
						if (busApp.setting['notification_status']) {
							busApp.notification({'bus_app_id':bus_app_id, 'debug':debug});
						}

						// Push
						if (busApp.setting['push_status']) {
							busApp.push({'bus_app_id':bus_app_id, 'debug':debug});
						}

						// Message
						busApp.message({'lang':busApp.setting['lang']});

						// Sync
						if (busApp.setting['sync_status']) {
							console.log(busApp.setting['sync_status']);
							busApp.sync();
						}
					}
				}).catch (function (error) {
					if (busApp.setting['debug'] == 1) {
						console.error('bus-app error:', error);
					}
				});
			} else {
				if (busApp.setting['debug']) {
					debug['serviceWorker'] = 'no support';
				}

				// Notification
				if (busApp.setting['notification_status']) {
					busApp.notification({'bus_app_id':bus_app_id, 'debug':debug});
				}
			}

			if (busApp.setting['debug']) {
				if ('onbeforeinstallprompt' in window) {
					debug['beforeinstallprompt'] = 'yes support';
				} else {
					debug['beforeinstallprompt'] = 'no support';
				}
				if ('caches' in self) {
					debug['Cache self'] = 'yes support';
				} else {
					debug['Cache self'] = 'no support';
				}
				if ('caches' in window) {
					debug['Cache window'] = 'yes support';
				} else {
					debug['Cache window'] = 'no support';
				}
				if ('CacheStorage' in self) {
					debug['Cache Storage self'] = 'yes support';
				} else {
					debug['Cache Storage self'] = 'no support';
				}
				if ('CacheStorage' in window) {
					debug['Cache Storage window'] = 'yes support';
				} else {
					debug['Cache Storage window'] = 'no support';
				}
				if ('applicationCache' in window) {
					debug['Application Cache'] = 'yes support';
				} else {
					debug['Application Cache'] = 'no support';
				}
				if ('Notification' in window) {
					debug['Notification'] = 'yes support';
				} else {
					debug['Notification'] = 'no support';
				}
				if ('PushManager' in window) {
					debug['Push'] = 'yes support';
				} else {
					debug['Push'] = 'no support';
				}
				if ('SyncManager' in window) {
					debug['Sync'] = 'yes support';
				} else {
					debug['Sync'] = 'no support';
				}

				var end = new Date().getTime();
				setTimeout(function() {
					if (busApp.setting['debug'] == 1 && bus_app_id) {
						var desc = bus_app_id.querySelector('[data-message="desc"]');
						if (desc) {
							debug['php'] = busApp.setting['debug_php'];
							debug['js'] = 'Время выполнения JS-скрипта ' + (end - start)/1000 + ' сек. или ' + (end - start) + ' мс.';
							delete debug['api'];
							desc.style['text-align'] = 'left';
							for (var i in debug) {
								desc.innerHTML += (desc.innerHTML ? '<br>' : '') + i + ': ' + debug[i] + ';';
							}
							bus_app_id.removeAttribute('hidden');
						}
					} else if (busApp.setting['debug'] == 2) {
						debug['proverka'] = {proverka:1,proverka:{proverka:3}};
						debug['php'] = busApp.setting['debug_php'];
						debug['js'] = 'Время выполнения JS-скрипта ' + (end - start)/1000 + ' сек. или ' + (end - start) + ' мс.';
						busApp.debug(debug);
					}
				}, busApp.setting['delay']+500);
			}

			var element = new CustomEvent('busAppAfter', {bubbles: true});
			document.dispatchEvent(element);
			return true;
		} else {
			return false;
		}
	},
	'Uint8ArrayTourlB64':function(Uint8Array) {
		//https://gist.github.com/enepomnyaschih/72c423f727d395eeaa09697058238727
		//JSON.parse(JSON.stringify())
		return btoa(String.fromCharCode.apply(null, new Uint8Array(Uint8Array)));
	},
	'urlB64ToUint8Array':function(base64String) {
		var padding = '='.repeat((4 - base64String.length % 4) % 4);
		var base64 = (base64String + padding).replace(/\-/g, '+').replace(/_/g, '/');
		var rawData = window.atob(base64);
		var outputArray = new Uint8Array(rawData.length);
		for (var i = 0; i < rawData.length; ++i) {
			outputArray[i] = rawData.charCodeAt(i);
		}
		if (busApp.setting['debug'] == 1) {
			console.log('id подписчика: ', outputArray);
		}
		return outputArray;
	},
	'fullscreen':function(element) {
		var element = document.body;

		if (element.requestFullscreen) {
			element.requestFullscreen();
		} else if (element.mozRequestFullScreen) {
			element.mozRequestFullScreen();
		} else if (element.webkitRequestFullscreen) {
			element.webkitRequestFullscreen();
		} else if (element.msRequestFullscreen) {
			element.msRequestFullscreen();
		} else if (element.MSFullscreenChange) {
			element.MSFullscreenChange();
		} else /* if (typeof window.ActiveXObject !== "undefined") {
			var wscript = new ActiveXObject("WScript.Shell");
			if (wscript !== null) {
				wscript.SendKeys("{F11}");
			}
		} else */ {
			window.open(window.location.href, '', 'fullscreen=yes, location=no, scrollbars=auto');
		}
		window.localStorage.setItem('bus-app-block-height', window.innerHeight);
	},
	'install':function(setting) {
		//busApp.debug('test2 ' + busApp.setting['browser']['name'] + ' ' + busApp.setting['browser']['version'], 1);
		var userAgent = window.navigator.userAgent.toLowerCase();
		var display = busApp.setting['display'];

		var desc = setting['bus_app_id'].querySelector('[data-message="desc"]');
		var button = setting['bus_app_id'].querySelector('[data-button="install"]');
		var ios = /macintosh|iphone|ipod|ipad/.test(userAgent);
		//userAgent = 'Mozilla/5.0 (Macintosh; Intel Mac OS X 11_1_0) AppleWebKit/536.30.1 (KHTML, like Gecko) Version/6.0.5 Safari/536.30.1';
		var ios_version = (userAgent.match(/\b[0-9]+_[0-9]+(?:_[0-9]+)?\b/)||[''])[0].replace(/_/g,'.');
		var android = /android/.test(userAgent);
		var x11 = /x11/.test(userAgent);
		var windows = /windows/.test(userAgent);
		if (ios) {
			var device = 'ios (macintosh, iphone, ipod, ipad)';
		} else if (android) {
			var device = 'android';
		} else if (x11) {
			var device = 'x11 - скорее полная версия сайта на android';
		} else if (windows) {
			var device = 'windows';
		} else {
			var device = 'not iOS, not android, not windows - напишите в тех. поддержку описав своё устройство';
		}

		if (setting['bus_app_id'] && display == 'browser') {
			if (ios && ios_version >= '11.1.0') {
				if (busApp.setting['description_ios'] && (!window.localStorage.getItem('bus-app-block') || (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime'])) {
					if (desc) {
						desc.innerHTML += (desc.innerHTML ? '<br>' : '') + busApp.setting['description_ios'] + ' ';
					}
					button.setAttribute('hidden', true);
					setting['bus_app_id'].style['top'] = 'unset';
					setting['bus_app_id'].style['bottom'] = 0;
					setTimeout(function() {
						setting['bus_app_id'].removeAttribute('hidden');
					}, busApp.setting['delay']);
				}
			} else if ((android || x11 || windows) && (busApp.setting['browser']['name'] == 'chrome' && busApp.setting['browser']['version'] >= '40.0' && busApp.setting['browser']['version'] < '66.0' || busApp.setting['browser']['name'] == 'huaweibrowser' || busApp.setting['browser']['name'] == 'miuibrowser')) {
				if (busApp.setting['description_android'] && (!window.localStorage.getItem('bus-app-block') || (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime'])) {
					if (desc) {
						desc.innerHTML += (desc.innerHTML ? '<br>' : '') + busApp.setting['description_android'] + ' ';
					}
					button.setAttribute('hidden', true);
					setTimeout(function() {
						setting['bus_app_id'].removeAttribute('hidden');
					}, busApp.setting['delay']);
				}
			} else {
				if (desc && busApp.setting['description_bookmarks'] && (!window.localStorage.getItem('bus-app-block') || (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime'])) {
					if (1 == 0 && 'bookmarks' in window) {
					} else {
						desc.innerHTML += (desc.innerHTML ? '<br>' : '') + busApp.setting['description_bookmarks'] + ' ';

						// Firefox <23
						if ('sidebar' in window && 'addPanel' in window.sidebar) {
							button.setAttribute('onclick', 'window.sidebar.addPanel(document.title, window.location.href, \'\'); busApp.fullscreen(); busApp.close(\'bus-app\');');
							button.removeAttribute('hidden');
						// Internet Explorer
						} else if('external' in window && 'AddFavorite' in window.external) {
							button.setAttribute('onclick', 'window.external.AddFavorite(window.location.href, document.title); busApp.fullscreen(); busApp.close(\'bus-app\');');
							button.removeAttribute('hidden');
						// Opera <15 и Firefox >23
						} else if ('opera' in window && 'print' in window || ('sidebar' in window) && !(window.sidebar instanceof Node)) {
							button.href = window.location.href;
							button.title = document.title;
							button.rel = 'sidebar';
							button.setAttribute('onclick', 'busApp.fullscreen(); busApp.close(\'bus-app\');');
							button.removeAttribute('hidden');
						// Для других браузеров (в основном WebKit) мы используем простое оповещение, чтобы информировать пользователей о том, что они могут добавлять в закладки с помощью ctrl + D / cmd + D
						} else {
							if (userAgent.indexOf('mac') != - 1 || windows) {
								button.setAttribute('hidden', true);
								if (userAgent.indexOf('mac') != - 1) {
									busApp.setting['description_bookmarks_key'] = busApp.setting['description_bookmarks_key'].replace('CTRL', 'Command/Cmd');
								}
								desc.innerHTML += (desc.innerHTML ? '<br>' : '') + busApp.setting['description_bookmarks_key'];
								window.addEventListener('keydown', function(e) {
									if (e.ctrlKey && (e.key == 'd' || e.key == 'D' || e.key == 'в' || e.key == 'В' || e.which == 68)) {
										busApp.fullscreen();
										busApp.close('bus-app');
									}
								}, false);
							} else if (android || x11) {
								button.href = window.location.href;
								button.title = document.title;
								button.rel = 'sidebar';
								button.setAttribute('onclick', 'busApp.fullscreen(); busApp.close(\'bus-app\');');
								button.removeAttribute('hidden');
							}
						}
					}

					setTimeout(function() {
						setting['bus_app_id'].removeAttribute('hidden');
					}, busApp.setting['delay']);
				}
			}
		} else {
			busApp.close('bus-app');
		}

		if (busApp.setting['debug']) {
			setting['debug']['device'] = device;
			setting['debug']['display-mode'] = display;
		}

		return setting['debug'];
	},
	'audio':function(url) {
		var getaudio = new Audio(url);
		getaudio.preload = 'auto';
		getaudio.autoplay = true;
		getaudio.volume = 1;
		//getaudio.muted = true;

		/* var hidden, state, visibilityChange; 
		if (typeof document.hidden !== "undefined") {
			hidden = "hidden";
			visibilityChange = "visibilitychange";
			state = "visibilityState";
		} else if (typeof document.mozHidden !== "undefined") {
			hidden = "mozHidden";
			visibilityChange = "mozvisibilitychange";
			state = "mozVisibilityState";
		} else if (typeof document.msHidden !== "undefined") {
			hidden = "msHidden";
			visibilityChange = "msvisibilitychange";
			state = "msVisibilityState";
		} else if (typeof document.webkitHidden !== "undefined") {
			hidden = "webkitHidden";
			visibilityChange = "webkitvisibilitychange";
			state = "webkitVisibilityState";
		}

		document.addEventListener(visibilityChange, function() {
			if (document[hidden]) {
				getaudio.play();
			} else {
				getaudio.pause();
			}
		}, false); */

		if (busApp.setting['debug'] == 1) {
			getaudio.addEventListener('play', function(event) {
				console.log('play');
			});
			getaudio.addEventListener('pause', function(event) {
				console.log('pause');
			});
			getaudio.addEventListener('canplay', function(event) {
				console.log('canplay');
			});
			getaudio.addEventListener('canplaythrough', function(event) {
				console.log('canplaythrough');
			});
			getaudio.addEventListener('playing', function(event) {
				console.log('playing');
			});
			getaudio.addEventListener('volumechange', function(event) {
				console.log('volumechange');
			});
			getaudio.addEventListener('emptied', function(event) {
				console.log('emptied');
			});
		}

		return true;
	},
	'notification':function(setting) {
		var button;
		if (setting['bus_app_id']) {
			button = setting['bus_app_id'].querySelector('[data-button="notification"]');
		}
		var status = false;

		if ('Notification' in window) {
			status = true;
			Notification = window.Notification;
		} else if ('mozNotification' in window) {
			Notification = window.mozNotification;
			status = true;
		} else if ('webkitNotifications' in window) {
			Notification = window.webkitNotifications;
			status = true;
		}

		if (status) {
			if (busApp.setting['debug'] == 1) {
				console.log('Разрешение Notification: ', Notification.permission);
			}

			var start = function() {
				busApp.ajax('index.php?route=' + busApp.setting['api'] + '/notification', {
					metod: 'POST',
					//data: {route:busApp.setting['route'],userAgent:window.navigator.userAgent},
					success:function(json) {
						if (busApp.setting['debug'] == 1) {
							console.log(json);
						}
						var time = 0;
						var not = [];
						var count = 0;
						var count_new = 0;

						for (var i in json) {
							for (var ii in json[i]) {
								var status = true;

								if (i == 'information' || i == 'article' || i == 'product') {
									for (var iii in json[i]) {
										if (window.localStorage.getItem('bus-app-notification-' + i + '-' + iii) == json[i][iii]['option']['tag']) {
											status = false;
										}
									}
								}
								if (i == 'cart' || i == 'wishlist' || i == 'compare') {
									if (window.localStorage.getItem('bus-app-notification-' + i + '-' + ii) && (Date.now() - window.localStorage.getItem('bus-app-notification-' + i + '-' + ii)) < (10*60*1000)) {
										status = false;
									}
								}

								if (status) {
									not[count] = {};
									not[count]['type'] = i;
									not[count]['count'] = ii;
									count++;

									if (busApp.setting['debug'] == 1) {
										console.log('array ' + i + ' ' + ii, json[i]);
										console.log('array2 ' + not[count_new]['type'] + ' ' + not[count_new]['count'], json[not[count_new]['type']][not[count_new]['count']]);
									}

									setTimeout(function() {
										if (busApp.setting['debug'] == 1) {
											console.log('array ' + i + ' ' + ii, json[i]);
											console.log('array2 ' + not[count_new]['type'] + ' ' + not[count_new]['count'], json[not[count_new]['type']][not[count_new]['count']]);
										}
										/* if (typeof nots !== 'undefined') {
											nots.close();
										} */

										/* window.navigator.serviceWorker.ready.then(function(registration) {
											registration.showNotification(json[not[count_new]['type']][not[count_new]['count']]['title'], json[not[count_new]['type']][not[count_new]['count']]['option']);
										}); */
										var nots = new Notification(json[not[count_new]['type']][not[count_new]['count']]['title'], json[not[count_new]['type']][not[count_new]['count']]['option']);
										nots['href'] = json[not[count_new]['type']][not[count_new]['count']]['href'];
										nots.addEventListener('click', function(event) {
											if (busApp.setting['debug'] == 1) {
												console.log('onclick', event.target.href);
											} else {
												if (event.target.href) {
													event.preventDefault();
													//window.location.href = event.target.href;
													window.open(event.target.href, '_blank', 'noopener,noreferrer');
												}
												event.target.close();
											}
										});
										nots.addEventListener('show', function(event) {
											if (busApp.setting['debug'] == 1) {
												console.log('onshow ' + i + ' ' + ii);
												console.log('onshow2 ' + not[count_new]['type'] + ' ' + not[count_new]['count']);
											}
											busApp.audio(busApp.setting['notification_audio']);
											if ((not[count_new]['type'] == 'information' || not[count_new]['type'] == 'article' || not[count_new]['type'] == 'product')) {
												window.localStorage.setItem('bus-app-notification-' + not[count_new]['type'] + '-' + not[count_new]['count'], event.target.tag);
											} else if (not[count_new]['type'] == 'cart' || not[count_new]['type'] == 'wishlist' || not[count_new]['type'] == 'compare') {
												window.localStorage.setItem('bus-app-notification-' + not[count_new]['type'] + '-' + not[count_new]['count'], Date.now());
											}
											count_new++;
										});
										nots.addEventListener('error', function(event) {
											if (busApp.setting['debug'] == 1) {
												console.log('onerror');
											}
										});
										nots.addEventListener('close', function(event) {
											if (busApp.setting['debug'] == 1) {
												console.log('onclose');
											}
										});
										document.addEventListener('visibilitychange', function() {
											if (document.visibilityState === 'visible') {
												nots.close();
											}
										});
									}, time);
									time += 9000;
								}
							}
						}
					},
					error: function(xhr, ajaxOptions, thrownError) {
						//alert('Error ' + xhr.status);
					}
				});
			};

			// Если разрешено, то создаем уведомление
			if (Notification.permission === 'granted') {
				if (button) {
					button.setAttribute('hidden', true);
				}

				start();
				setInterval(start, busApp.setting['notification_interval']*1000);
			} else {
				// отправка в первый раз
				if (button) {
					if (!window.localStorage.getItem('bus-app-block') || window.localStorage.getItem('bus-app-block') && (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime']) {
						var desc = setting['bus_app_id'].querySelector('[data-message="desc"]');
						if (desc) {
							desc.innerHTML = (desc.innerHTML > '' ? desc.innerHTML + '<br>' : '') + busApp.setting['description_notification'] + ' ';
							button.removeAttribute('hidden');
							setTimeout(function() {
								setting['bus_app_id'].removeAttribute('hidden');
							}, busApp.setting['delay']);
						}
					}

					button.addEventListener('click', function(event) {
						var checkNotificationPromise = function() {
							try {
								Notification.requestPermission().then();
							} catch(error) {
								return false;
							}

							return true;
						}
						if (checkNotificationPromise()) {
							Notification.requestPermission(function(permission) {
							}).then(function(permission) {
								if (permission === 'granted') {
									if (event.target) {
										event.target.setAttribute('hidden', true);
									}
									setting['bus_app_id'].setAttribute('hidden', true);

									start();
								}
							});
						} else {
							Notification.requestPermission(function(permission) {
								if (permission === 'granted') {
									if (event.target) {
										event.target.setAttribute('hidden', true);
									}
									setting['bus_app_id'].setAttribute('hidden', true);

									start();
								}
							});
						}

						if (Notification.permission === 'denied' && busApp.setting['notification_error']) {
							var line = document.querySelector('#bus-app-line');

							if (line) {		
								var desc = line.querySelector('[data-message]');
								if (desc) {
									desc.setAttribute('data-message', 'offline');
									desc.innerHTML = busApp.setting['notification'];
								}
								var box = line.querySelector('[data-box]')
								if (box) {
									box.style['min-height'] = '60px';
								}
								line.removeAttribute('hidden');
								setTimeout(function() {
									line.setAttribute('hidden', true);
								}, 10000);
							}
						}
					});
				}
			}

			var element = new CustomEvent('busAppNotification', {bubbles: true});
			document.dispatchEvent(element);
		} else {
			if (button) {
				button.setAttribute('hidden', true);
			}
		}
	},
	'push':function(setting) {
		// пуш старой мозилы 2012 года
		/* if (busApp.setting['debug'] == 1 && 'requestRemotePermission' in Notification) {
			var request = Notification.requestRemotePermission();
			request.onsuccess = function() {
				var url = request.result;
				console.log('New push URL: ' + url);
				//Сохраняет URL-идентификатор на своём сервере
				//jQuery.post('/push-urls/', {url: url});
			};
		} */

		var button;
		if (setting['bus_app_id']) {
			button = setting['bus_app_id'].querySelector('[data-button="notification"]');
		}
		var status = false;

		if ('PushManager' in window) {
			status = true;
		}

		if (status && window.navigator.serviceWorker) {
			window.navigator.serviceWorker.ready.then(function(registration) {
				registration.onpush = function(event) {
					if (busApp.setting['debug'] == 1) {
						console.log('onpush', event.data);
					}
				}
				registration.pushManager.getSubscription().then(function(pushSubscription) {
					if (pushSubscription !== null) {
						console.log('Bus_app User IS subscribed.');
						if (busApp.setting['debug'] == 1) {
							console.log('Результат подписки ', JSON.parse(JSON.stringify(pushSubscription)));
						}
					} else {
						console.log('Bus_app User is NOT subscribed.');
						if ('Notification' in window) {
							Notification = window.Notification;
						} else if ('mozNotification' in window) {
							Notification = window.mozNotification;
						} else if ('webkitNotifications' in window) {
							Notification = window.webkitNotifications;
						}

						if (button) {
							button.addEventListener('click', function(event) {
								var checkNotificationPromise = function() {
									try {
										Notification.requestPermission().then();
									} catch(error) {
										return false;
									}

									return true;
								}
								if (checkNotificationPromise()) {
									Notification.requestPermission(function(permission) {
									}).then(function(permission) {
										if (permission === 'granted') {
											if (event.target) {
												event.target.setAttribute('hidden', true);
											}
											if (setting['bus_app_id']) {
												setting['bus_app_id'].setAttribute('hidden', true);
											}
										}
									});
								} else {
									Notification.requestPermission(function(permission) {
										if (permission === 'granted') {
											if (event.target) {
												event.target.setAttribute('hidden', true);
											}
											if (setting['bus_app_id']) {
												setting['bus_app_id'].setAttribute('hidden', true);
											}
										}
									});
								}

								if (!busApp.setting['notification_status'] && Notification.permission === 'denied' && busApp.setting['notification_error']) {
									var line = document.querySelector('#bus-app-line');

									if (line) {		
										var desc = line.querySelector('[data-message]');
										if (desc) {
											desc.setAttribute('data-message', 'offline');
											desc.innerHTML = busApp.setting['notification'];
										}
										var box = line.querySelector('[data-box]')
										if (box) {
											box.style['min-height'] = '60px';
										}
										line.removeAttribute('hidden');
										setTimeout(function() {
											line.setAttribute('hidden', true);
										}, 10000);
									}
								}

								if (busApp.setting['push_service'] == 1) {
									var messaging = firebase.messaging();
									if ('usePublicVapidKey' in messaging) {
										//messaging.usePublicVapidKey(busApp.setting['push_public_key']);
									}
									if ('useServiceWorker' in messaging) {
										messaging.useServiceWorker(registration);
									}
									var api = messaging.getToken({
										serviceWorkerRegistration: registration,
										vapidKey: busApp.setting['push_public_key']
									}).then(function(token) {
										return {endpoint:token};
									});
								} else {
									var api = registration.pushManager.subscribe({
										userVisibleOnly: true,
										applicationServerKey: busApp.urlB64ToUint8Array(busApp.setting['push_public_key'])
									})
								}

								api.then(function(subscription) {
									var api = JSON.parse(JSON.stringify(subscription));

									if (busApp.setting['debug'] == 1) {
										console.log('Результат подписки ', api);
									}

									busApp.ajax('index.php?route=' + busApp.setting['api'] + '/push', {
										metod: 'POST',
										data: {api:api},
										success:function(json, xhr) {
											if (typeof json === 'object') {
												for (var i in json) {
													registration.showNotification(json[i][0]['title'], json[i][0]['option']);
												}
												busApp.audio(busApp.setting['notification_audio']);
											}
										},
										error: function(xhr, ajaxOptions, thrownError) {
											//alert('Error ' + xhr.status);

											console.error('Bus_app Push Невозможно получить данные с сервера: ', xhr);

											registration.showNotification('Ошибочка вышла', {
												body: 'Мы хотели сообщить вам что-то важное, но у нас всё сломалось.',
												icon: 'image/catalog/bus_app/favicon-192x192.png',
												tag: 'notification-error',
												//data: {url:'llll'}, //the url which we gonna use later
												//actions: [{action: "open_url", title: "Read Now"}]
											});
											busApp.audio(busApp.setting['notification_audio']);
										}
									});
								}).catch(function(error) {
									if (busApp.setting['debug'] == 1) {
										console.log('Failed to subscribe the user: ', error);
									}
								});
							});

							if (Notification.permission === 'granted') {
								button.click();
							} else {
								if (!busApp.setting['notification_status']) {
									if (!window.localStorage.getItem('bus-app-block') || window.localStorage.getItem('bus-app-block') && (Date.now() - window.localStorage.getItem('bus-app-block')) > busApp.setting['closeTime']) {
										var desc = setting['bus_app_id'].querySelector('[data-message="desc"]');
										if (desc) {
											desc.innerHTML = (desc.innerHTML > '' ? desc.innerHTML + '<br>' : '') + busApp.setting['description_notification'] + ' ';
											button.removeAttribute('hidden');
											setTimeout(function() {
												setting['bus_app_id'].removeAttribute('hidden');
											}, busApp.setting['delay']);
										}
									}
								}
							}
						} else {
							if (Notification.permission === 'granted') {
								if (busApp.setting['push_service'] == 1) {
									var messaging = firebase.messaging();
									if ('usePublicVapidKey' in messaging) {
										//messaging.usePublicVapidKey(busApp.setting['push_public_key']);
									}
									if ('useServiceWorker' in messaging) {
										messaging.useServiceWorker(registration);
									}
									var api = messaging.getToken({
										serviceWorkerRegistration: registration,
										vapidKey: busApp.setting['push_public_key']
									}).then(function(token) {
										return {endpoint:token};
									});
								} else {
									var api = registration.pushManager.subscribe({
										userVisibleOnly: true,
										applicationServerKey: busApp.urlB64ToUint8Array(busApp.setting['push_public_key'])
									})
								}

								api.then(function(subscription) {
									var api = JSON.parse(JSON.stringify(subscription));

									if (busApp.setting['debug'] == 1) {
										console.log('Результат подписки ', api);
									}

									busApp.ajax('index.php?route=' + busApp.setting['api'] + '/push', {
										metod: 'POST',
										data: {api:api},
										success:function(json, xhr) {
											/* if (typeof json === 'object') {
												for (var i in json) {
													registration.showNotification(json[i][0]['title'], json[i][0]['option']);
												}
												busApp.audio(busApp.setting['notification_audio']);
											} */
										},
										error: function(xhr, ajaxOptions, thrownError) {
											//alert('Error ' + xhr.status);

											console.error('Bus_app Push Невозможно получить данные с сервера: ', xhr);

											registration.showNotification('Ошибочка вышла', {
												body: 'Мы хотели сообщить вам что-то важное, но у нас всё сломалось.',
												icon: 'image/catalog/bus_app/favicon-192x192.png',
												tag: 'notification-error',
												//data: {url:'llll'}, //the url which we gonna use later
												//actions: [{action: "open_url", title: "Read Now"}]
											});
											busApp.audio(busApp.setting['notification_audio']);
										}
									});
								}).catch(function(error) {
									if (busApp.setting['debug'] == 1) {
										console.log('Failed to subscribe the user: ', error);
									}
								});
							}
						}
					}

					var element = new CustomEvent('busAppPush', {bubbles: true});
					document.dispatchEvent(element);
				});
			});
		}
	},
	'message':function(message) {
		if (window.navigator.serviceWorker) {
			window.navigator.serviceWorker.addEventListener('message', function(event) {
				if (busApp.setting['debug'] == 1) {
					console.log('The service worker sent me a message: ', event.data);
				}
			});

			window.navigator.serviceWorker.ready.then(function(registration) {
				registration.active.postMessage(message);
			});
		}
	},
	'sync':function() {
		if ('SyncManager' in window && window.navigator.serviceWorker) {
			console.log('serviceWorker: ', window.navigator.serviceWorker.ready);
			window.navigator.serviceWorker.ready.then(function(reg) {
				var setting = {
					'id':'bus-app',
					'idleRequired':false,
					'allowOnBattery':true,
					'maxDelay':0,
					'minDelay':0,
					'minPeriod':0,
					'minRequiredNetwork':'network-online', //'network-any', 'network-offline', 'network-online', 'network-non-mobile'
				};
				
				console.log('sync: ', reg.sync.register(setting['id']));
				return ;
				
			}).catch(function() {
				// system was unable to register for a sync,
				// this could be an OS-level restriction
				//postDataFromThePage();
			});
		}
	},
	'cache': {
		'add':function(name, url, update) {
			if (url.indexOf('https') == -1) {
				return 'blocked http';
			}
			var url_exception = new URL(url);
			if (url_exception.pathname && url_exception.pathname != '/' && busApp.setting['cache_resources_exception'].indexOf(url_exception.pathname) != -1 || url_exception.hostname && busApp.setting['cache_resources_exception'].indexOf(url_exception.hostname) != -1) {
				//console.log('Исключение из кэша: ' + url);
				return 'exception ' + url;
			}
			//var new_url = url.split('?');
			//url = new_url[0];
			if (typeof update === 'undefined') {
				var update = false;
			}
			// нужно доработать по параметрам
			window.caches.open(name).then(function(cache) {
				if (update) {
					cache.delete(url);

					//return cache.add(url);
					window.fetch(url).then(function(response) {
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
						window.fetch(url).then(function(response) {
							return cache.put(url, response);
						});
					});
				}
			});
		},
		'addAll':function(name, urls) {
			//window.caches.delete(name);
			//удаление повторов нужно доработать по параметрам
			/* var new_urls = {};
			var new_urls = urls.filter(obj => !new_urls[obj] && (new_urls[obj] = true)); */
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
			//console.log('new_urls', new_urls);
			window.caches.open(name).then(function(cache) {
				cache.addAll(new_urls).catch(function(error) {
					if (busApp.setting['debug'] == 1) {
						console.log('error:', error);
					}
				});
			}).catch(function(error) {
				//console.log(error);
			});
		},
		'delete':function(name, url) {
			if (typeof name === 'undefined') {
				var name = 'bus-app-' + busApp.setting['cache_token'];
			}
			if (typeof url === 'undefined') {
				var url = window.location.href;
			}
			window.fetch(url, {
				//mode: 'same-origin',
				referrerPolicy: 'no-referrer-when-downgrade',
				cache: 'reload'
			}).then(function(response) {
				//console.log(url);
				window.caches.open(name).then(function(cache) {
					//console.log(response);
					cache.delete(url, response)/* .then(busShowList) */;
				});
			}).catch(function(error) {
				//console.log(error);
			});
		}
	},
	'blockUpdate':function(block, url) {
		window.fetch(url).then(function (response) {
			return response.text();
		}).then(function (data) {
			//console.log(data);
			var blockSelect = document.querySelector(block);
			if (blockSelect) {
				blockSelect.innerHTML = data;
			}
		});
	},
	'close':function(id) {
		document.getElementById(id).setAttribute('hidden', true);
		window.localStorage.setItem(id + '-block', Date.now());
	},
	//aHR0cHM6Ly9sZWFybi5qYXZhc2NyaXB0LnJ1L211dGF0aW9uLW9ic2VydmVy
	'domObserver':function(el, callback) {
		var done = function() {
			callback(el);
		};
		var MutationObserver = window.MutationObserver || window.WebKitMutationObserver || window.MozMutationObserver;

		if (MutationObserver) {
		var observer = new MutationObserver(done);
			observer.observe(el, { childList: true });
		} else if (el.addEventListener) {
			el.addEventListener('DOMNodeInserted', done, false); 
		} else {
			var html = el.innerHTML;
			setInterval(function() {
				if (html != el.innerHTML) {
					html = el.innerHTML;
					done();
				}
			}, 300);
		}
	},
	'md5':function(d){result = busApp.md5res.M(busApp.md5res.V(busApp.md5res.Y(busApp.md5res.X(d),8*d.length)));return result.toLowerCase()},
	'md5res':{
		'M':function(d){for(var _,m="0123456789ABCDEF",f="",r=0;r<d.length;r++)_=d.charCodeAt(r),f+=m.charAt(_>>>4&15)+m.charAt(15&_);return f},
		'X':function(d){for(var _=Array(d.length>>2),m=0;m<_.length;m++)_[m]=0;for(m=0;m<8*d.length;m+=8)_[m>>5]|=(255&d.charCodeAt(m/8))<<m%32;return _},
		'V':function(d){for(var _="",m=0;m<32*d.length;m+=8)_+=String.fromCharCode(d[m>>5]>>>m%32&255);return _},
		'Y':function(d,_){d[_>>5]|=128<<_%32,d[14+(_+64>>>9<<4)]=_;for(var m=1732584193,f=-271733879,r=-1732584194,i=271733878,n=0;n<d.length;n+=16){var h=m,t=f,g=r,e=i;f=busApp.md5res.md5_ii(f=busApp.md5res.md5_ii(f=busApp.md5res.md5_ii(f=busApp.md5res.md5_ii(f=busApp.md5res.md5_hh(f=busApp.md5res.md5_hh(f=busApp.md5res.md5_hh(f=busApp.md5res.md5_hh(f=busApp.md5res.md5_gg(f=busApp.md5res.md5_gg(f=busApp.md5res.md5_gg(f=busApp.md5res.md5_gg(f=busApp.md5res.md5_ff(f=busApp.md5res.md5_ff(f=busApp.md5res.md5_ff(f=busApp.md5res.md5_ff(f,r=busApp.md5res.md5_ff(r,i=busApp.md5res.md5_ff(i,m=busApp.md5res.md5_ff(m,f,r,i,d[n+0],7,-680876936),f,r,d[n+1],12,-389564586),m,f,d[n+2],17,606105819),i,m,d[n+3],22,-1044525330),r=busApp.md5res.md5_ff(r,i=busApp.md5res.md5_ff(i,m=busApp.md5res.md5_ff(m,f,r,i,d[n+4],7,-176418897),f,r,d[n+5],12,1200080426),m,f,d[n+6],17,-1473231341),i,m,d[n+7],22,-45705983),r=busApp.md5res.md5_ff(r,i=busApp.md5res.md5_ff(i,m=busApp.md5res.md5_ff(m,f,r,i,d[n+8],7,1770035416),f,r,d[n+9],12,-1958414417),m,f,d[n+10],17,-42063),i,m,d[n+11],22,-1990404162),r=busApp.md5res.md5_ff(r,i=busApp.md5res.md5_ff(i,m=busApp.md5res.md5_ff(m,f,r,i,d[n+12],7,1804603682),f,r,d[n+13],12,-40341101),m,f,d[n+14],17,-1502002290),i,m,d[n+15],22,1236535329),r=busApp.md5res.md5_gg(r,i=busApp.md5res.md5_gg(i,m=busApp.md5res.md5_gg(m,f,r,i,d[n+1],5,-165796510),f,r,d[n+6],9,-1069501632),m,f,d[n+11],14,643717713),i,m,d[n+0],20,-373897302),r=busApp.md5res.md5_gg(r,i=busApp.md5res.md5_gg(i,m=busApp.md5res.md5_gg(m,f,r,i,d[n+5],5,-701558691),f,r,d[n+10],9,38016083),m,f,d[n+15],14,-660478335),i,m,d[n+4],20,-405537848),r=busApp.md5res.md5_gg(r,i=busApp.md5res.md5_gg(i,m=busApp.md5res.md5_gg(m,f,r,i,d[n+9],5,568446438),f,r,d[n+14],9,-1019803690),m,f,d[n+3],14,-187363961),i,m,d[n+8],20,1163531501),r=busApp.md5res.md5_gg(r,i=busApp.md5res.md5_gg(i,m=busApp.md5res.md5_gg(m,f,r,i,d[n+13],5,-1444681467),f,r,d[n+2],9,-51403784),m,f,d[n+7],14,1735328473),i,m,d[n+12],20,-1926607734),r=busApp.md5res.md5_hh(r,i=busApp.md5res.md5_hh(i,m=busApp.md5res.md5_hh(m,f,r,i,d[n+5],4,-378558),f,r,d[n+8],11,-2022574463),m,f,d[n+11],16,1839030562),i,m,d[n+14],23,-35309556),r=busApp.md5res.md5_hh(r,i=busApp.md5res.md5_hh(i,m=busApp.md5res.md5_hh(m,f,r,i,d[n+1],4,-1530992060),f,r,d[n+4],11,1272893353),m,f,d[n+7],16,-155497632),i,m,d[n+10],23,-1094730640),r=busApp.md5res.md5_hh(r,i=busApp.md5res.md5_hh(i,m=busApp.md5res.md5_hh(m,f,r,i,d[n+13],4,681279174),f,r,d[n+0],11,-358537222),m,f,d[n+3],16,-722521979),i,m,d[n+6],23,76029189),r=busApp.md5res.md5_hh(r,i=busApp.md5res.md5_hh(i,m=busApp.md5res.md5_hh(m,f,r,i,d[n+9],4,-640364487),f,r,d[n+12],11,-421815835),m,f,d[n+15],16,530742520),i,m,d[n+2],23,-995338651),r=busApp.md5res.md5_ii(r,i=busApp.md5res.md5_ii(i,m=busApp.md5res.md5_ii(m,f,r,i,d[n+0],6,-198630844),f,r,d[n+7],10,1126891415),m,f,d[n+14],15,-1416354905),i,m,d[n+5],21,-57434055),r=busApp.md5res.md5_ii(r,i=busApp.md5res.md5_ii(i,m=busApp.md5res.md5_ii(m,f,r,i,d[n+12],6,1700485571),f,r,d[n+3],10,-1894986606),m,f,d[n+10],15,-1051523),i,m,d[n+1],21,-2054922799),r=busApp.md5res.md5_ii(r,i=busApp.md5res.md5_ii(i,m=busApp.md5res.md5_ii(m,f,r,i,d[n+8],6,1873313359),f,r,d[n+15],10,-30611744),m,f,d[n+6],15,-1560198380),i,m,d[n+13],21,1309151649),r=busApp.md5res.md5_ii(r,i=busApp.md5res.md5_ii(i,m=busApp.md5res.md5_ii(m,f,r,i,d[n+4],6,-145523070),f,r,d[n+11],10,-1120210379),m,f,d[n+2],15,718787259),i,m,d[n+9],21,-343485551),m=busApp.md5res.safe_add(m,h),f=busApp.md5res.safe_add(f,t),r=busApp.md5res.safe_add(r,g),i=busApp.md5res.safe_add(i,e)}return Array(m,f,r,i)},
		'md5_cmn':function(d,_,m,f,r,i){return busApp.md5res.safe_add(busApp.md5res.bit_rol(busApp.md5res.safe_add(busApp.md5res.safe_add(_,d),busApp.md5res.safe_add(f,i)),r),m)},
		'md5_ff':function(d,_,m,f,r,i,n){return busApp.md5res.md5_cmn(_&m|~_&f,d,_,r,i,n)},
		'md5_gg':function(d,_,m,f,r,i,n){return busApp.md5res.md5_cmn(_&f|m&~f,d,_,r,i,n)},
		'md5_hh':function(d,_,m,f,r,i,n){return busApp.md5res.md5_cmn(_^m^f,d,_,r,i,n)},
		'md5_ii':function(d,_,m,f,r,i,n){return busApp.md5res.md5_cmn(m^(_|~f),d,_,r,i,n)},
		'safe_add':function(d,_){var m=(65535&d)+(65535&_);return(d>>16)+(_>>16)+(m>>16)<<16|65535&m},
		'bit_rol':function(d,_){return d<<_|d>>>32-_},
	},
	'ajax':function(url, setting) {
		if (typeof url == 'object') {
			setting = url;
			if (typeof setting['url'] === 'undefined') {
				return false;
			} else {
				url = setting['url'];
			}
		}
		if (typeof setting['type'] !== 'undefined') {
			setting['metod'] = setting['type'];
		}
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
		if (typeof setting['beforeSend'] === 'undefined') {
			setting['beforeSend'] = function() {};
		}
		if (typeof setting['success'] === 'undefined') {
			setting['success'] = function(json) {};
		}
		if (typeof setting['error'] === 'undefined') {
			setting['error'] = function(error) {};
		}
		if (typeof setting['complete'] === 'undefined') {
			setting['complete'] = function(json) {};
		}
		if (typeof setting['debug'] === 'undefined') {
			setting['debug'] = false;
		}
		var xhr = new XMLHttpRequest();
		setting['beforeSend'](xhr, setting);
		var datanew = null;
		if (setting['data']) {
			var i, i2, i3;
			if (typeof setting['data'] == 'object' && ('val' in setting['data'] || 'values' in setting['data'])) {
				datanew = {};
				for (i in setting['data']) {
					if (isNaN(i) == false) {
						datanew[setting['data'][i]['name']] = setting['data'][i]['value'];
					}
				}
				setting['data'] = datanew;
				setting['dataType'] = 'text';
			}

			if (setting['dataType'] == 'json') {
				datanew = JSON.stringify(setting['data']);
			} else {
				if (typeof FormData !== 'undefined') {
					datanew = new FormData();
					if (typeof setting['data'] == 'object') {
						for (i in setting['data']) {
							if (typeof setting['data'][i] == 'object') {
								for (i2 in setting['data'][i]) {
									if (typeof setting['data'][i][i2] == 'object') {
										for (i3 in setting['data'][i][i2]) {
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
						for (i in setting['data']) {
							if (typeof setting['data'][i] == 'object') {
								for (i2 in setting['data'][i]) {
									if (typeof setting['data'][i][i2] == 'object') {
										for (i3 in setting['data'][i][i2]) {
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
			xhr.responseType = setting['responseType']; //\"text\" – строка,\"arraybuffer\", \"blob\", \"document\", \"json\" – JSON (парсится автоматически).
		}
		if (setting['debug']) {
			console.log('xhr data: ', datanew);
		}
		xhr.send(datanew);
		xhr.onload = function(oEvent) {
			if (xhr.status == 200) {
				setting['success'](xhr.response, xhr);
				setting['complete'](xhr, setting, xhr.response);
			} else {
				setting['error'](xhr, setting, false);
				setting['complete'](xhr, setting, false);
			}
			return xhr;
		};

		return true;
	},
	'debug':function(message, block) {
		if (typeof block == 'undefined') {
			block = false;
		}
		if (block) {
			var line = document.querySelector('#bus-app-line');

			if (line) {
				var desc = line.querySelector('[data-message]');
				if (desc) {
					desc.setAttribute('data-message', 'online');
					desc.innerHTML = message;
				}
				var box = line.querySelector('[data-box]')
				if (box) {
					box.style['min-height'] = '60px';
				}
				line.removeAttribute('hidden');
				setTimeout(function() {
					line.setAttribute('hidden', true);
				}, 5000);
			}
		} else {
			busApp.ajax('index.php?route=' + busApp.setting['api'] + '/debug', {
				metod: 'POST',
				data: {debug:message},
				success:function(json, xhr) {
					//console.log(json);
					//window.localStorage.setItem('bus-app-debug', true);
				},
				error: function(xhr, ajaxOptions, thrownError) {
					//alert('Error ' + xhr.status);
				}
			});
		}
	}
};

// запуск скрипта
if (document.readyState == 'loading') {
	//document.addEventListener('DOMContentLoaded', busApp.start, {once:true, passive:true});
	window.addEventListener('load', busApp.start, {once:true, passive:true});
}
if (document.readyState == 'interactive') {
	window.addEventListener('load', busApp.start, {once:true, passive:true});
}
if (document.readyState == 'complete') {
	window.addEventListener('pagehide', busApp.start, {once:true, passive:true});
	window.addEventListener('scroll', busApp.start, {once:true, passive:true});
	window.addEventListener('mouseover', busApp.start, {once:true, passive:true});
	window.addEventListener('touchstart', busApp.start, {once:true, passive:true});
}