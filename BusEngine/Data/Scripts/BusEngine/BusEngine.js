/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */
'use strict';
//'use asm';
/* Chrome error off/block cookie "'Window': Access is denied for this document." */
try {
	window.localStorage.length;
	window.localStorage.status = true;
	window.sessionStorage.length;
	window.sessionStorage.status = true;
} catch (e) {
	delete window.localStorage;
	delete window.sessionStorage;
	window.localStorage = {
		'status':false,
		'getItem':function(a) {},
		'setItem':function(a, b) {},
		'key':function(a) {},
		'removeItem':function(a) {},
		'clear':function() {},
		'length':0
	};
	window.sessionStorage = window.localStorage;
} finally {

}

/* window.addEventListener('load', function(e) {
	if (!window.localStorage.status && !window.sessionStorage.status && !document.cookie) {
		setTimeout(function() {
			window.alert('Каб выкарыстоўваць усе магчымасці сайта, неабходна ўключыць cookie.');
		}, 1000);
	}
}); */

// Fix Internet Explorer
if (!window.console) {
	window.console = {};
	window.console.log = window.console.assert = function(){};
	window.console.warn = window.console.assert = function(){};
}

// contextmenu off Internet Explorer 8+
/* document.oncontextmenu = function () {
	return false;
}; */

if (!('BusEngine' in window)) {
	window.BusEngine = {};
}

window.console.logs = window.console.log;
BusEngine.log = window.console.log = function(args) {
	args = Array.prototype.slice.call(arguments);
	var log, i, l;

	l = new Error().stack;

	if (l) {
		l = l.split('\n');
		if ('length' in l && l.length > 0) {
			//l = l[l.length-2].match(new RegExp('(?<=\\().*?(?=\\))'));
			l = l[l.length-1].substring(l[l.length-1].indexOf('at ')+3);
			if (l) {
				args.push(l);
			}
		}
	}

	log = '';

	for (i = 0; i < args.length; ++i) {
		if (typeof args[i] == 'object') {
			log += (log ? ' ' : '') + window.JSON.stringify(args[i]);
		} else {
			log += (log ? ' ' : '') + args[i];
		}
	}

	window.console.logs(log);
};

BusEngine.tools = {};
BusEngine.tools.ajax = function(url, setting) {
	if (typeof url == 'object') {
		setting = url;
		if (typeof setting['url'] === 'undefined') {
			return false;
		} else {
			url = setting['url'];
		}
	}

	if (typeof setting['headers'] !== 'object') {
		setting['headers'] = {};
	}

	if (typeof setting['type'] === 'string') {
		setting['method'] = setting['type'];
	}

	if (typeof setting['method'] !== 'string') {
		setting['method'] = 'GET';
	}

	if (typeof setting['responseType'] !== 'string') {
		setting['responseType'] = 'json';
	}

	if (typeof setting['dataType'] !== 'string') {
		setting['dataType'] = 'text';
	}

	if (typeof setting['data'] === 'undefined') {
		setting['data'] = '';
	}

	if (typeof setting['async'] !== 'boolean') {
		setting['async'] = true;
	}

	if (typeof setting['user'] === 'undefined') {
		setting['user'] = null;
	}

	if (typeof setting['password'] === 'undefined') {
		setting['password'] = null;
	}

	if (typeof setting['beforeSend'] !== 'function') {
		setting['beforeSend'] = function() {};
	}

	if (typeof setting['success'] !== 'function') {
		setting['success'] = function() {};
	}

	if (typeof setting['error'] !== 'function') {
		setting['error'] = function() {};
	}

	if (typeof setting['complete'] !== 'function') {
		setting['complete'] = function() {};
	}

	if (typeof setting['debug'] === 'boolean') {
		setting['debug'] = false;
	}

	var i, datanew = null, xhr = new XMLHttpRequest();

	setting['beforeSend'](xhr, setting);

	if (setting['data']) {
		if (setting['dataType'] == 'json') {
			datanew = JSON.stringify(setting['data']);
		} else {
			if (typeof setting['data'] == 'object') {
				var arrayData, arrayDatas = function(data, gi) {
					var i, ii, iii, array, arrayg;

					array = {};

					for (i in data) {
						if (gi) {
							ii = gi + '[' + encodeURIComponent(i) + ']';
						} else {
							ii = encodeURIComponent(i);
						}
						if (typeof data[i] == 'object') {
							arrayg = arrayDatas(data[i], ii);
							for (iii in arrayg) {
								array[iii] = encodeURIComponent(arrayg[iii]);
							}
						} else {
							array[ii] = encodeURIComponent(data[i]);
						}
					}

					return array;
				}

				arrayData = arrayDatas(setting['data']);

				if ('FormData' in window) {
					datanew = new FormData();

					for (i in arrayData) {
						datanew.append(i, arrayData[i]);
					}
				} else {
					datanew = [];

					for (i in arrayData) {
						datanew.push(i + '=' + arrayData[i]);
					}

					datanew = datanew.join('&').replace(/%20/g, '+');
				}
			} else {
				datanew = setting['data'];
			}
		}
	}

	xhr.open(setting['method'], url, setting['async'], setting['user'], setting['password']);
	xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
	if (!('FormData' in window)) {
		if (setting['dataType'] == 'json') {
			xhr.setRequestHeader('Content-type', 'application/json; charset=UTF-8');
		} else if (setting['dataType'] == 'text') {
			xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded; charset=UTF-8');
		}
	}

	for (i in setting['headers']) {
		xhr.setRequestHeader(i, setting['headers'][i]);
	}

	if (setting['responseType']) {
		xhr.responseType = setting['responseType']; //\"text\" – строка,\"arraybuffer\", \"blob\", \"document\", \"json\" – JSON (парсится автоматически).
	}

	if (setting['debug']) {
		console.log('xhr data: ', datanew);
	}

	xhr.onload = function(e) {
		if (e.target.status == 200) {
			setting['success'](e.target.response, e.target);
			setting['complete'](e.target, setting, e.target.response);
		} else {
			setting['error'](e.target, setting, false);
			setting['complete'](e.target, setting, false);
		}
	};
	xhr.send(datanew);

	return xhr;
};

if (!('postMessage' in window.BusEngine)) {
	window.BusEngine.postMessage = function(m) {
		BusEngine.tools.ajax({
			url: 'post_message.php',
			method: 'POST',
			responseType: 'text',
			data: {post_message: m}
		});
	};
}

BusEngine.open = function(url, node1, node2, params, callback) {
	if ('href' in this && this.href) {
		this.preventDefault();
		if (typeof url != 'string' || !url) {
			url = this.href;
		}
	}

	if (!('body' in document)) {
		return this;
	}

	if (typeof node1 !== 'string') {
		node1 = 'main';
	}

	if (typeof node2 !== 'string') {
		node2 = 'main';
	}

	var method = 'GET';

	if (typeof params !== 'undefined' && typeof params !== 'function') {
		method = 'POST';
	}

	if (typeof params !== 'function') {
		callback = function() {};
	}

	document.body.classList.add('be-open');

	BusEngine.tools.ajax({
		url: url,
		method: method,
		responseType: 'document',
		data: params,
		success: function(data, xhr) {
			var e, element = document.querySelector(node1);

			if (!element) {
				element = document.documentElement;
			}

			if (element) {
				e = data.querySelector(node2);

				if (!e) {
					e = data;
				}

				if (e && 'innerHTML' in e) {
					var i, l, m, stylesheets, scripts, script;

					// fix memory chrome
					m = element.querySelectorAll('video, audio');
					l = m.length;
					for (i = 0; i < l; ++i) {
						m[i].src = '';
						m[i].parentNode.removeChild(m[i]);
					}

					// замена html вариант 1
					/* if (e.hasChildNodes()) {
						l = e.childNodes.length-1;

						for (i = l; i > 0; --i) {
							if (e.childNodes[i].nodeType == 1) {
								if (e.childNodes[i].tagName == 'SCRIPT') {
									script = document.createElement('script');
									if (e.childNodes[i].text) {
										script.text = e.childNodes[i].text;
									}
									if (e.childNodes[i].src) {
										script.src = e.childNodes[i].src;
									}
									document.body.appendChild(script).parentNode.removeChild(script);
								}
								element.prepend(e.childNodes[i]);
							} else {
								element.prepend(e.childNodes[i].textContent);
							}
						}
					} */

					// замена html вариант 2
					//element.innerHTML = e.innerHTML;
					element.innerHTML = '';
					element.insertAdjacentHTML('beforeEnd', e.innerHTML);

					// стили из страницы
					stylesheets = data.getElementsByTagName('link');

					if (stylesheets) {
						l = stylesheets.length;

						for (i = 0; i < l; ++i) {
							if (stylesheets[i].href && [window.location.href, window.location.protocol + '//' + window.location.hostname + '/'].indexOf(stylesheets[i].href) == -1) {
								BusEngine.loadStyle(stylesheets[i].href);
							}
						}
					}

					// скрипты из страницы
					scripts = data.querySelectorAll('script');

					if (scripts) {
						l = scripts.length;

						for (i = 0; i < l; ++i) {
							if (scripts[i].src) {
								BusEngine.loadScript(scripts[i].src);
							}
						}
					}

					// скрипты в пределах html
					scripts = e.querySelectorAll('script');

					if (scripts) {
						l = scripts.length;

						for (i = 0; i < l; ++i) {
							if (scripts[i].text || scripts[i].src) {
								script = document.createElement('script');
								if (scripts[i].type) {
									script.type = scripts[i].type;
								}
								if (scripts[i].text) {
									script.text = scripts[i].text;
								}
								if (scripts[i].src) {
									script.src = scripts[i].src;
								}
								document.head.appendChild(script).parentNode.removeChild(script);
							}
						}
					}
				}
			}

			callback(e);
		},
		error: function(data) {
			BusEngine.log(data);
		},
		complete: function(data) {
			document.body.classList.remove('be-open');
		}
	});
};

BusEngine.tools.json = {};
BusEngine.tools.json.encode = window.JSON.stringify;
BusEngine.tools.json.decode = window.JSON.parse;

if (!('engine' in window.BusEngine)) {
	BusEngine.engine = {
		'settingEngine': {},
		'settingProject': {},
	};
}

/* if (window.location.hostname != 'bd.busengine') {
	BusEngine.tools.ajax({
		url: 'BusEngine.json',
		success: function(data) {
			if ('settingProject' in data) {
				for (var i in data['settingProject']) {
					BusEngine.engine.settingProject[i] = data[i];
				}
			}
		}
	});
} */

BusEngine.basename = function(path, suffix) {
	path = path.replace(/\\\\/g, '/');
	var s = path.lastIndexOf('/');

	if (s != -1) {
		var ss = path.length;

		if (suffix) {
			ss = path.lastIndexOf(suffix, ss);
			if (ss == -1) {
				ss = path.length;
			}
		}

		path = path.substring(s+1, ss);
	}

	return path;
}

BusEngine.loadStyle = function(url, callback, setting) {
	var status, s;
	s = document.querySelector('link[href*="' + url + '"], link[href*="' + BusEngine.basename(url) + '"]');
	status = s;

	if (status) {
		if (typeof callback == 'function') {
			if (s.getAttribute('data-start')) {
				s.addEventListener('readystatechange', callback);
				s.addEventListener('load', callback);
			} else {
				setTimeout(callback, 1);
			}
		}
	} else {
		s = document.createElement('link');
		s.href = url;
		s.type = 'text/css';
		s.rel = 'stylesheet';

		if (typeof callback == 'function') {
			s.addEventListener('readystatechange', function() {
				s.removeAttribute('data-start');
			});
			s.addEventListener('load', function() {
				s.removeAttribute('data-start');
			});
			s.addEventListener('readystatechange', callback);
			s.addEventListener('load', callback);
		}
	}

	if (typeof setting == 'object') {
		if (Symbol.toStringTag in setting && setting[Symbol.toStringTag] == 'NamedNodeMap') {
			var i, l;
			l = setting.length;

			for (i = 0; i < l; ++i) {
				s.setAttribute(setting[i].name, setting[i].value);
			}
		} else {
			for (var i in setting) {
				s.setAttribute(i, setting[i]);
			}
		}
	}

	if (!status && 'head' in document) {
		document.head.appendChild(s);
	}

	return s;
};

BusEngine.loadScript = function(url, callback, setting) {
	var status, s;
	s = document.querySelector('script[src*="' + url + '"], script[src*="' + BusEngine.basename(url) + '"]');
	status = s;

	if (status) {
		if (typeof callback == 'function') {
			if (s.getAttribute('data-start')) {
				s.addEventListener('readystatechange', callback);
				s.addEventListener('load', callback);
			} else {
				setTimeout(callback, 1);
			}
		}
	} else {
		s = document.createElement('script');
		s.src = url;
		s.type = 'text/javascript';
		s.setAttribute('data-start', true);

		if (typeof callback == 'function') {
			s.addEventListener('readystatechange', function() {
				s.removeAttribute('data-start');
			});
			s.addEventListener('load', function() {
				s.removeAttribute('data-start');
			});
			s.addEventListener('readystatechange', callback);
			s.addEventListener('load', callback);
		}
	}

	if (typeof setting == 'object') {
		if (Symbol.toStringTag in setting && setting[Symbol.toStringTag] == 'NamedNodeMap') {
			var i, l;
			l = setting.length;

			for (i = 0; i < l; ++i) {
				s.setAttribute(setting[i].name, setting[i].value);
			}
		} else {
			for (var i in setting) {
				s.setAttribute(i, setting[i]);
			}
		}
	}

	if (!status && 'head' in document) {
		document.head.appendChild(s);
	}

	return s;
};

if (!('localization' in window.BusEngine)) {
	BusEngine.localization = {};
}

if (!('getLanguages' in window.BusEngine.localization)) {
	BusEngine.localization.getLanguages = {};
}
BusEngine.localization.isFiles = {};
BusEngine.localization.load = function(language, file) {
	if (typeof language != 'string') {
		language = document.documentElement.lang;
	}
	if (typeof file != 'string') {
		file = false;
	}
	var load = function(language, file) {
		var time = new Date().getTime();
		var i, i2, i3, l, l2, l3, langs, langs2, langs3, k, gl;
		langs = document.getElementsByTagName("*");
		l = langs.length;

		for (i = 0; i < l; ++i) {
			langs2 = langs[i].childNodes;
			l2 = langs2.length;

			for (i2 = 0; i2 < l2; ++i2) {
				if (langs2[i2].nodeType == Node.TEXT_NODE) {
					for (i3 in BusEngine.localization.getLanguages) {
						langs2[i2].data = langs2[i2].data.replace(new RegExp(i3, 'gim'), BusEngine.localization.getLanguages[i3]);
					}
				} else if (langs2[i2].nodeType == Node.ELEMENT_NODE) {
					if (['HEAD', 'LINK', 'BODY', 'HTML', 'SOURCE'].indexOf(langs2[i2].tagName) == -1) {
						k = langs2[i2].getAttribute('data-localization');

						if (k && k in BusEngine.localization.getLanguages) {
							gl = BusEngine.localization.getLanguages[k];
						} else {
							gl = false;
						}

						langs3 = langs2[i2].attributes;
						l3 = langs3.length;

						for (i3 = 0; i3 < l3; ++i3) {
							if (['rel', 'type', 'class', 'data-src', 'src', 'name', 'value'].indexOf(langs3[i3].nodeName) == -1) {
								if (langs3[i3].nodeName == 'data-localization' && langs3[i3].value == k && gl) {
									//langs2[i2].value = gl;
									langs2[i2].innerText = gl;
								} else {
									if (!gl && langs3[i3].value in BusEngine.localization.getLanguages) {
										k = langs3[i3].value;
										gl = BusEngine.localization.getLanguages[k];
									}

									if (gl) {
										langs3[i3].value = langs3[i3].value.replace(new RegExp('\\b' + k + '$', 'i'), gl);
									}
								}
							}
						}
					}
				}
			}
		}

		time = new Date().getTime() - time;
		console.logs('Время обработки localization: ' + time/1000 + ' сек. или ' + time + ' мс.', BusEngine.localization.getLanguages);
	};

	if (window.location.hostname != 'bd.busengine') {
		if (!Object.keys(BusEngine.localization.getLanguages) || !file && language in BusEngine.localization.isFiles || file && file in BusEngine.localization.isFiles) {
			load(language, file);

			return false;
		} else {
			BusEngine.localization.isFiles[language] = true;
			if (file) {
				BusEngine.localization.isFiles[file] = true;
			}
		}

		BusEngine.tools.ajax({
			url: ('content' in BusEngine.engine.settingProject && 'localization' in BusEngine.engine.settingProject['content'] ? BusEngine.engine.settingProject['content']['localization'] : 'Localization') + '/' + language + (file ? '/' + file : '') + '.json',
			success: function(data) {
				for (var i in data) {
					BusEngine.localization.getLanguages[i] = data[i];
				}
				load(language, file);
			}
		});
	} else {
		load(language, file);
	}
};
/* if (document.readyState != 'loading') {
	BusEngine.localization.load();
} else {
	window.addEventListener('DOMContentLoaded', function() {
		BusEngine.localization.load();
	});
} */
BusEngine.localization.getLanguage = function(key) {
	if (Object.hasOwn(BusEngine.localization.getLanguages, key)) {
		return BusEngine.localization.getLanguages[key];
	} else {
		return key;
	}
};
BusEngine.localization.setLanguage = function(key, value) {
	BusEngine.localization.getLanguages[key] = value;
};

// https://developer.mozilla.org/ru/docs/Web/API/HTMLMediaElement
BusEngine.polyfillTagSource = function(ex) {
	if (typeof ex == 'undefined') {
		ex = [];
	}
	var i, l, v = document.querySelectorAll('video source[media]:not([data-error])');
	l = v.length;

	for (i = 0; i < l; ++i) {
		if (window.matchMedia(v[i].media).matches) {
			if (v[i].getAttribute('data-src') && ex.indexOf(v[i].getAttribute('data-src')) == -1) {
				v[i].setAttribute('src', v[i].getAttribute('data-src'));
				v[i].removeAttribute('data-src');
				v[i].parentNode.addEventListener('error', function(e) {
					e.target.setAttribute('data-error', e.target.src);
					ex.push(e.target.src);
					BusEngine.polyfillTagSource(ex);
				});
				v[i].parentNode.src = v[i].getAttribute('src');
				break;
			}
		} else {
			if (v[i].getAttribute('src')) {
				v[i].setAttribute('data-src', v[i].getAttribute('src'));
				v[i].removeAttribute('src');
			}
		}
	}
};

BusEngine.cookie = {
	'set': function(name, value, domain, path, day) {
		if (typeof name != 'string') {
			return false;
		}

		if (typeof value != 'string') {
			value = '';
		}

		if (typeof domain == 'object' && domain != null) {
			if ('path' in domain) {
				path = domain.path;
			}
			if ('day' in domain) {
				day = domain.day;
			}
			if ('domain' in domain) {
				domain = domain.domain;
			}
		}

		if (typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path != 'string') {
			path = '/';
		}

		if (typeof day != 'undefined' && 'Date' in window) {
			var x = new window.Date();
			x.setUTCSeconds(3600 * 24 * Number(day));
			day = ' expires=' + x + ';';
		} else {
			day = '';
		}

		document.cookie = name + '=' + value + '; path=' + path + ';' + day + ' domain=' + domain;

		return true;
	},
	'get': function(name) {
		var c = document.cookie;

		if (!c || typeof name != 'string') {
			return c;
		}

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (c && c[2]) {
			return c[2];
		} else {
			return '';
		}
	},
	'remove': function(name, value, domain, path) {
		if (typeof name != 'string') {
			return false;
		}

		var v;

		if (typeof value != 'string') {
			v = '';
		} else {
			v = value;
		}

		if (typeof domain == 'object' && domain != null) {
			if ('path' in domain) {
				path = domain.path;
			}
			if ('domain' in domain) {
				domain = domain.domain;
			}
		}

		if (typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path != 'string') {
			path = '/';
		}

		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=';
		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=' + domain;

		return true;
	},
	'has': function(name, value) {
		var c = document.cookie;

		if (!c || typeof name != 'string') {
			return false;
		}

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (typeof value != 'string') {
			if (c && c[1] && c[1] == name) {
				return true;
			}
		} else {
			if (c && c[2] && c[2] == value) {
				return true;
			}
		}

		return false;
	},
	'test': function() {
		// добавить
		BusEngine.cookie.set('BusEngine', 'Like');
		console.log(document.cookie);

		// получить
		console.log(BusEngine.cookie.get('BusEngine'));

		// проверить
		console.log(BusEngine.cookie.has('BusEngine'));

		// удалить
		BusEngine.cookie.remove('BusEngine');
		console.log(document.cookie);

		// проверить
		console.log(BusEngine.cookie.has('BusEngine'));
	}
};

// делаем код под стиль c#
BusEngine.PostMessage = BusEngine.postMessage;
BusEngine.Log = BusEngine.log;
BusEngine.Engine = BusEngine.engine;
BusEngine.Engine.SettingEngine = BusEngine.engine.settingEngine;
BusEngine.Engine.SettingProject = BusEngine.engine.settingProject;
BusEngine.Localization = BusEngine.localization;
BusEngine.Localization.Load = BusEngine.localization.load;
BusEngine.Localization.GetLanguages = BusEngine.localization.getLanguages;
BusEngine.Localization.GetLanguage = BusEngine.localization.getLanguage;
BusEngine.Localization.SetLanguage = BusEngine.localization.setLanguage;
BusEngine.PolyfillTagSource = BusEngine.polyfillTagSource;
BusEngine.Cookie = BusEngine.cookie;
BusEngine.LoadScript = BusEngine.loadScript;
BusEngine.Loadstyle = BusEngine.loadStyle;
BusEngine.Open = BusEngine.open;
BusEngine.Tools = BusEngine.tools;
BusEngine.Tools.Ajax = BusEngine.tools.ajax;
BusEngine.Tools.Json = BusEngine.tools.json;
BusEngine.Tools.Json.Encode = BusEngine.tools.json.encode;
BusEngine.Tools.Json.Decode = BusEngine.tools.json.decode;