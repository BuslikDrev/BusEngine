/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */
'use strict';
'use asm';
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
if (!('postMessage' in window.BusEngine)) {
	window.BusEngine.postMessage = function(m) {};
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

if (!('engine' in window.BusEngine)) {
	BusEngine.engine = {
		'settingEngine': {},
		'settingProject': {},
	};
}

BusEngine.loadScript = function(url, callback, setting) {
	var s = document.createElement('script');
	s.src = url;
	s.type = 'text/javascript';
	if (typeof callback != 'undefined') {
		s.onreadystatechange = callback;
		s.onload = callback;
	}
	if (typeof setting == 'object') {
		for (var ss in setting) {
			s.setAttribute(ss, setting[ss]);
		}
	}
	if ('head' in document) {
		document.head.appendChild(s);
	}
};

BusEngine.loadStyle = function(url, callback, setting) {
	var s = document.createElement('link');
	s.href = url;
	s.type = 'text/css';
	if (typeof callback != 'undefined') {
		s.onreadystatechange = callback;
		s.onload = callback;
	}
	if (typeof setting == 'object') {
		for (var ss in setting) {
			s.setAttribute(ss, setting[ss]);
		}
	}
	if ('head' in document) {
		document.head.appendChild(s);
	}
};

if (!('localization' in window.BusEngine)) {
	BusEngine.localization = {};
}

if (!('getLanguages' in window.BusEngine.localization)) {
	BusEngine.localization.getLanguages = {};
}

window.addEventListener('DOMContentLoaded', function() {
	if (window.location.host != 'busengine') {
		import(window.location.href.substring(0, window.location.href.lastIndexOf('/', window.location.href.length)+1) + 'Localization/' + document.documentElement.lang + '.js').then(function(module) {
			if (typeof module.default == 'object') {
				for (var i in module.default) {
					BusEngine.localization.getLanguages[i] = module.default[i];
				}

				BusEngine.localization.initialize();
			}
		});
	} else {
		BusEngine.localization.initialize();
	}
});

BusEngine.localization.initialize = function() {
	var i4, i3, l3, langs3, i2, l2, langs2, i, l, langs;
	langs = document.getElementsByTagName("*");
	l = langs.length;

	for (i = 0; i < l; ++i) {
		langs2 = langs[i].childNodes;
		l2 = langs2.length;

		for (i2 = 0; i2 < l2; ++i2) {
			if (langs2[i2].nodeType == Node.TEXT_NODE) {
				for (i4 in BusEngine.localization.getLanguages) {
					langs2[i2].data = langs2[i2].data.replace(new RegExp('' + String(i4).replace(/([\\\-[\]{}()*+?.,^$|])/g, '\\$1') + '', 'gim'), BusEngine.localization.getLanguages[i4]);
				}
			} else if (langs2[i2].nodeType == Node.ELEMENT_NODE) {
				for (i4 in BusEngine.localization.getLanguages) {
					if ('attributes' in langs2[i2] && ['HEAD', 'LINK', 'BODY', 'HTML'].indexOf(langs2[i2].tagName) == -1) {
						//console.logs(langs2[i2].tagName);
						langs3 = langs2[i2].attributes;
						l3 = langs3.length;

						for (i3 = 0; i3 < l3; ++i3) {
							if (['rel', 'type', 'src', 'href', 'class'].indexOf(langs3[i3].nodeName) == -1) {
								langs3[i3].value = langs3[i3].value.replace(new RegExp('' + String(i4).replace(/([\\\-[\]{}()*+?.,^$|])/g, '\\$1') + '$', 'i'), BusEngine.localization.getLanguages[i4]);
								if (langs3[i3].nodeName == 'data-localization') {
									//window.console.logs(langs3[i3]);
									langs2[i2].value = langs3[i3].value;
									langs2[i2].innerText = langs3[i3].value;
									//langs3[i2].value = langs3[i3].value;
								}
							}
						}
					}
				}
			}
		}
	}
};
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
		if (typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		if (typeof value == 'undefined' || typeof value != 'string') {
			value = '';
		}

		if (typeof domain == 'object' && domain != null) {
			console.log(domain);
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

		if (typeof domain == 'undefined' || typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path == 'undefined' || typeof path != 'string') {
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

		if (!c || typeof name == 'undefined' || typeof name != 'string') {
			return c;
		}

		//console.log(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));
		//console.log(/(name)\=(\S[^\;]+)/);

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (c && c[2]) {
			return c[2];
		} else {
			return '';
		}
	},
	'remove': function(name, value, domain, path) {
		if (typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		var v;

		if (typeof value == 'undefined' || typeof value != 'string') {
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

		if (typeof domain == 'undefined' || typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path == 'undefined' || typeof path != 'string') {
			path = '/';
		}

		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=';
		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=' + domain;

		return true;
	},
	'has': function(name, value) {
		var c = document.cookie;

		if (!c || typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (typeof value == 'undefined' || typeof value != 'string') {
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
	if (typeof setting['type'] !== 'undefined') {
		setting['method'] = setting['type'];
	}
	if (typeof setting['method'] === 'undefined') {
		setting['method'] = 'GET';
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
	if (typeof setting['debug'] === 'undefined') {
		setting['debug'] = false;
	}

	var datanew = null, xhr = new XMLHttpRequest();
	setting['beforeSend'](xhr, setting);

	if (setting['data']) {
		var i, i2, i3;
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
	if (setting['responseType']) {
		xhr.responseType = setting['responseType']; //\"text\" – строка,\"arraybuffer\", \"blob\", \"document\", \"json\" – JSON (парсится автоматически).
	}
	if (setting['debug']) {
		console.log('xhr data: ', datanew);
	}
	xhr.onload = function(e) {
		//console.logs(e);
		if (e.target.status == 200) {
			setting['success'](e.target.response, e.target);
			setting['complete'](e.target, setting, e.target.response);
		} else {
			setting['error'](e.target, setting, false);
			setting['complete'](e.target, setting, false);
		}
	};
	xhr.send(datanew);
	xhr = null;

	return xhr;
};

/* BusEngine.tools.ajax({
	url:'https://busengine.buslikdrev.by/',
	method:'post',
	data:{'один':true, 'два':{'три':false,'три':[1,1]}},
	debug: true,
	success: function(data) {
		//console.log(data);
	}
}); */

BusEngine.open = function(url, node1, node2, params, callback) {
	if (!('body' in document)) {
		return this;
	}

	if ('href' in this && this.href) {
		this.preventDefault();
		if (typeof url != 'string' || !url) {
			url = this.href;
		}
	}

	if (typeof node1 !== 'string') {
		node1 = 'main';
	}

	if (typeof node2 !== 'string') {
		node2 = 'main';
	}

	var method = 'GET';

	if (typeof params !== 'undefined') {
		method = 'POST';
	}

	if (typeof params !== 'function') {
		callback = function() {};
	}

	document.body.classList.add('be-open');

	BusEngine.tools.ajax({
		url: url,
		ajax: true,
		method: method,
		responseType: 'document',
		data: params,
		success: function(data, xhr) {
			var e, element = document.querySelector(node1);

			if (!element) {
				element = document.documentElement;
			}

			if (element) {
				/* var div = document.createElement('html');
				div.innerHTML = data;
				data = div; */

				e = data.querySelector(node2);

				if (!e) {
					e = data;
				}

				if (e) {
					var i, l, m, scripts, script;

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
					scripts = e.querySelectorAll('script');

					if (scripts) {
						l = scripts.length;

						for (i = 0; i < l; ++i) {
							if (scripts[i].text || scripts[i].src) {
								script = document.createElement('script');
								/* if (scripts[i].type) {
									script.type = scripts[i].type;
								} */
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

					//window.history.pushState(null, null, xhr.responseURL);
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

// делаем код под стиль c#
BusEngine.PostMessage = BusEngine.postMessage;
BusEngine.Log = BusEngine.log;
BusEngine.Engine = BusEngine.engine;
BusEngine.Engine.SettingEngine = BusEngine.engine.settingEngine;
BusEngine.Engine.SettingProject = BusEngine.engine.settingProject;
BusEngine.Localization = BusEngine.localization;
BusEngine.Localization.Initialize = BusEngine.localization.initialize;
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