(function() {
	function ha(a, b) {
		a.prototype = Me(b.prototype);
		a.prototype.constructor = a;
		if (hc) hc(a, b);
		else for (var c in b) if ("prototype" != c) if (Object.defineProperties) {
			var d = Object.getOwnPropertyDescriptor(b, c);
			d && Object.defineProperty(a, c, d)
		} else a[c] = b[c];
		a.Fa = b.prototype
	}
	function Qa(a, b) {
		if (b) {
			var c = Ne;
			a = a.split(".");
			for (var d = 0; d < a.length - 1; d++) {
				var e = a[d];
				e in c || (c[e] = {});
				c = c[e]
			}
			a = a[a.length - 1];
			d = c[a];
			b = b(d);
			b != d && null != b && Oe(c, a, {
				configurable: !0,
				writable: !0,
				value: b
			})
		}
	}
	function aa(a) {
		return "string" == typeof a
	}
	function ic(a) {
		return "boolean" == typeof a
	}
	function D(a) {
		return "number" == typeof a
	}
	function jc() {
		if (null === nb) {
			var a = l.document;
			(a = a.querySelector && a.querySelector("script[nonce]")) && (a = a.nonce || a.getAttribute("nonce")) && Pe.test(a) || (a = null);
			nb = a || ""
		}
		return nb
	}
	function ob() {}
	function ba(a) {
		var b = typeof a;
		if ("object" == b) if (a) {
			if (a instanceof Array) return "array";
			if (a instanceof Object) return b;
			var c = Object.prototype.toString.call(a);
			if ("[object Window]" == c) return "object";
			if ("[object Array]" == c || "number" == typeof a.length && "undefined" != typeof a.splice && "undefined" != typeof a.propertyIsEnumerable && !a.propertyIsEnumerable("splice")) return "array";
			if ("[object Function]" == c || "undefined" != typeof a.call && "undefined" != typeof a.propertyIsEnumerable && !a.propertyIsEnumerable("call")) return "function"
		} else return "null";
		else if ("function" == b && "undefined" == typeof a.call) return "object";
		return b
	}
	function Qe(a) {
		return null === a
	}
	function Ra(a) {
		var b = typeof a;
		return "object" == b && null != a || "function" == b
	}
	function Re(a, b, c) {
		return a.call.apply(a.bind, arguments)
	}
	function Se(a, b, c) {
		if (!a) throw Error();
		if (2 < arguments.length) {
			var d = Array.prototype.slice.call(arguments, 2);
			return function() {
				var c = Array.prototype.slice.call(arguments);
				Array.prototype.unshift.apply(c, d);
				return a.apply(b, c)
			}
		}
		return function() {
			return a.apply(b, arguments)
		}
	}
	function ia(a, b, c) {
		Function.prototype.bind && -1 != Function.prototype.bind.toString().indexOf("native code") ? ia = Re : ia = Se;
		return ia.apply(null, arguments)
	}
	function Sa(a, b) {
		var c = Array.prototype.slice.call(arguments, 1);
		return function() {
			var b = c.slice();
			b.push.apply(b, arguments);
			return a.apply(this, b)
		}
	}
	function U(a, b) {
		function c() {}
		c.prototype = b.prototype;
		a.Fa = b.prototype;
		a.prototype = new c;
		a.prototype.constructor = a;
		a.Pa = function(a, c, f) {
			for (var d = Array(arguments.length - 2), e = 2; e < arguments.length; e++) d[e - 2] = arguments[e];
			return b.prototype[c].apply(a, d)
		}
	}
	function Ta(a, b) {
		if (aa(a)) return aa(b) && 1 == b.length ? a.indexOf(b, 0) : -1;
		for (var c = 0; c < a.length; c++) if (c in a && a[c] === b) return c;
		return -1
	}
	function kc(a, b) {
		for (var c = a.length, d = aa(a) ? a.split("") : a, e = 0; e < c; e++) e in d && b.call(void 0, d[e], e, a)
	}
	function Te(a) {
		return Array.prototype.concat.apply([], arguments)
	}
	function pb(a) {
		return /^[\s\u00a0]*([\s\S]*?)[\s\u00a0]*$/.exec(a)[1]
	}
	function lc(a) {
		if (!Ue.test(a)) return a; - 1 != a.indexOf("&") && (a = a.replace(Ve, "&amp;")); - 1 != a.indexOf("<") && (a = a.replace(We, "&lt;")); - 1 != a.indexOf(">") && (a = a.replace(Xe, "&gt;")); - 1 != a.indexOf('"') && (a = a.replace(Ye, "&quot;")); - 1 != a.indexOf("'") && (a = a.replace(Ze, "&#39;")); - 1 != a.indexOf("\x00") && (a = a.replace($e, "&#0;"));
		return a
	}
	function E(a, b) {
		return -1 != a.indexOf(b)
	}
	function af(a) {
		return String(a).replace(/\-([a-z])/g, function(a, c) {
			return c.toUpperCase()
		})
	}
	function bf(a, b) {
		for (var c in a) if (b.call(void 0, a[c], c, a)) return !0;
		return !1
	}
	function za(a) {
		za[" "](a);
		return a
	}
	function Q() {}
	function V(a, b, c) {
		a.a = null;
		b || (b = []);
		a.A = void 0;
		a.h = -1;
		a.b = b;
		a: {
			if (b = a.b.length) {
				--b;
				var d = a.b[b];
				if (d && "object" == typeof d && "array" != ba(d) && !(cf && d instanceof Uint8Array)) {
					a.i = b - a.h;
					a.g = d;
					break a
				}
			}
			a.i = Number.MAX_VALUE
		}
		a.w = {};
		if (c) for (b = 0; b < c.length; b++) d = c[b], d < a.i ? (d += a.h, a.b[d] = a.b[d] || ja) : (mc(a), a.g[d] = a.g[d] || ja)
	}
	function mc(a) {
		var b = a.i + a.h;
		a.b[b] || (a.g = a.b[b] = {})
	}
	function v(a, b) {
		if (b < a.i) {
			b += a.h;
			var c = a.b[b];
			return c === ja ? a.b[b] = [] : c
		}
		if (a.g) return c = a.g[b], c === ja ? a.g[b] = [] : c
	}
	function Aa(a, b) {
		if (b < a.i) {
			b += a.h;
			var c = a.b[b];
			return c === ja ? a.b[b] = [] : c
		}
		c = a.g[b];
		return c === ja ? a.g[b] = [] : c
	}
	function nc(a, b) {
		a = v(a, b);
		return null == a ? !1 : a
	}
	function qa(a, b, c) {
		a.a || (a.a = {});
		if (!a.a[c]) {
			var d = v(a, c);
			d && (a.a[c] = new b(d))
		}
		return a.a[c]
	}

	function oc(a, b, c) {
		a.a || (a.a = {});
		if (!a.a[c]) {
			for (var d = Aa(a, c), e = [], f = 0; f < d.length; f++) e[f] = new b(d[f]);
			a.a[c] = e
		}
		b = a.a[c];
		b == ja && (b = a.a[c] = []);
		return b
	}
	function pc(a) {
		if (a.a) for (var b in a.a) {
			var c = a.a[b];
			if ("array" == ba(c)) for (var d = 0; d < c.length; d++) c[d] && qb(c[d]);
			else c && qb(c)
		}
	}
	function qb(a) {
		pc(a);
		return a.b
	}
	function df(a, b) {
		if (15 == b) {
			if (728 <= a) return 728;
			if (468 <= a) return 468
		} else if (90 == b) {
			if (200 <= a) return 200;
			if (180 <= a) return 180;
			if (160 <= a) return 160;
			if (120 <= a) return 120
		}
		return null
	}
	function ef() {
		return function() {
			return !Qe.apply(this, arguments)
		}
	}
	function qc(a) {
		var b = !1,
			c;
		return function() {
			b || (c = a(), b = !0);
			return c
		}
	}
	function ff() {
		var a = ob;
		return function() {
			if (a) {
				var b = a;
				a = null;
				b()
			}
		}
	}
	function ra() {
		this.b = "";
		this.h = rc
	}
	function rb(a) {
		if (a instanceof ra && a.constructor === ra && a.h === rc) return a.b;
		ba(a);
		return "type_error:TrustedResourceUrl"
	}
	function ka() {
		this.I = "";
		this.oa = sc
	}
	function tc(a) {
		var b = new ka;
		b.I = a;
		return b
	}
	function uc(a, b) {
		a.src = rb(b);
		(b = jc()) && a.setAttribute("nonce", b)
	}
	function Ba(a) {
		this.a = a || l.document || document
	}
	function Ua(a, b) {
		return a.a.createElement(String(b))
	}
	function vc(a) {
		gf();
		var b = new ra;
		b.b = a;
		return b
	}
	function sb() {
		return !(E(H, "iPad") || E(H, "Android") && !E(H, "Mobile") || E(H, "Silk")) && (E(H, "iPod") || E(H, "iPhone") || E(H, "Android") || E(H, "IEMobile"))
	}
	function M(a) {
		try {
			var b;
			if (b = !! a && null != a.location.href) a: {
				try {
					za(a.foo);
					b = !0;
					break a
				} catch (c) {}
				b = !1
			}
			return b
		} catch (c) {
			return !1
		}
	}
	function hf(a) {
		for (var b = l, c = 0; b && 40 > c++ && (!M(b) || !a(b));) b = wc(b)
	}
	function xc() {
		var a = l;
		hf(function(b) {
			a = b;
			return !1
		});
		return a
	}
	function wc(a) {
		try {
			var b = a.parent;
			if (b && b != a) return b
		} catch (c) {}
		return null
	}
	function yc(a, b) {
		var c = [l.top],
			d = [],
			e = 0;
		b = b || 1024;
		for (var f; f = c[e++];) {
			a && !M(f) || d.push(f);
			try {
				if (f.frames) for (var g = f.frames.length, h = 0; h < g && c.length < b; ++h) c.push(f.frames[h])
			} catch (k) {}
		}
		return d
	}
	function tb(a, b) {
		var c = a.createElement("script");
		uc(c, vc(b));
		return (a = a.getElementsByTagName("script")[0]) && a.parentNode ? (a.parentNode.insertBefore(c, a), c) : null
	}
	function N(a, b) {
		return b.getComputedStyle ? b.getComputedStyle(a, null) : a.currentStyle
	}
	function zc(a) {
		if (!a.crypto) return Math.random();
		try {
			var b = new Uint32Array(1);
			a.crypto.getRandomValues(b);
			return b[0] / 65536 / 65536
		} catch (c) {
			return Math.random()
		}
	}
	function ub(a, b) {
		if (a) for (var c in a) Object.prototype.hasOwnProperty.call(a, c) && b.call(void 0, a[c], c, a)
	}
	function vb(a) {
		var b = a.length;
		if (0 == b) return 0;
		for (var c = 305419896, d = 0; d < b; d++) c ^= (c << 5) + (c >> 2) + a.charCodeAt(d) & 4294967295;
		return 0 < c ? c : 4294967296 + c
	}
	function Ac(a) {
		return wb.test(a) && (a = Number(a), !isNaN(a)) ? a : null
	}
	function Ca(a, b) {
		return b ? !/^false$/.test(a) : /^true$/.test(a)
	}
	function A(a) {
		return (a = xb.exec(a)) ? +a[1] : null
	}
	function Bc(a, b) {
		try {
			return !(!a.frames || !a.frames[b])
		} catch (c) {
			return !1
		}
	}
	function Da(a, b, c) {
		a.addEventListener && a.addEventListener(b, c, !1)
	}
	function yb(a, b) {
		var c = !1,
			d = !1;
		d = void 0 === d ? !1 : d;
		c = void 0 === c ? !1 : c;
		l.google_image_requests || (l.google_image_requests = []);
		var e = l.document.createElement("img");
		if (b || c) {
			var f = function(a) {
					b && b(a);
					if (c) {
						a = l.google_image_requests;
						var d = Ta(a, e);
						0 <= d && Array.prototype.splice.call(a, d, 1)
					}
					e.removeEventListener && e.removeEventListener("load", f, !1);
					e.removeEventListener && e.removeEventListener("error", f, !1)
				};
			Da(e, "load", f);
			Da(e, "error", f)
		}
		d && (e.referrerPolicy = "no-referrer");
		e.src = a;
		l.google_image_requests.push(e)
	}
	function Cc(a, b) {
		a = parseInt(a, 10);
		return isNaN(a) ? b : a
	}
	function zb(a, b) {
		return a ? (a = a.match(jf)) ? a[0] : b : b
	}
	function Dc() {
		return zb("", "googleads.g.doubleclick.net")
	}
	function Ec(a) {
		return a ? "pagead2.googlesyndication.com" : zb("", "pagead2.googlesyndication.com")
	}
	function Ea(a) {
		a = void 0 === a ? l : a;
		var b = a.context || a.AMP_CONTEXT_DATA;
		if (!b) try {
			b = a.parent.context || a.parent.AMP_CONTEXT_DATA
		} catch (c) {}
		try {
			if (b && b.pageViewId && b.canonicalUrl) return b
		} catch (c) {}
		return null
	}
	function Ab(a) {
		return (a = a || Ea()) ? M(a.master) ? a.master : null : null
	}
	function ca(a, b) {
		for (var c in a) Object.prototype.hasOwnProperty.call(a, c) && b.call(void 0, a[c], c, a)
	}
	function la(a) {
		return !(!a || !a.call) && "function" === typeof a
	}
	function Va(a, b) {
		if (a.indexOf) return a = a.indexOf(b), 0 < a || 0 === a;
		for (var c = 0; c < a.length; c++) if (a[c] === b) return !0;
		return !1
	}
	function kf(a) {
		a = Ab(Ea(a)) || a;
		a.google_unique_id ? ++a.google_unique_id : a.google_unique_id = 1
	}
	function sa() {
		if (Fc && !M(ta)) {
			var a = "." + W.domain;
			try {
				for (; 2 < a.split(".").length && !M(ta);) W.domain = a = a.substr(a.indexOf(".") + 1), ta = window.parent
			} catch (b) {}
			M(ta) || (ta = window)
		}
		return ta
	}
	function X(a) {
		a = da && a.google_top_window || a.top;
		return M(a) ? a : null
	}
	function u(a, b) {
		a = a.google_ad_modifications;
		return Va(a ? a.eids || [] : [], b)
	}
	function Wa(a) {
		return (a = a.google_ad_modifications) ? a.loeids || [] : []
	}
	function Gc(a, b) {
		a = a.google_ad_modifications = a.google_ad_modifications || {};
		(a.tag_partners = a.tag_partners || []).push(b)
	}
	function Hc(a, b, c) {
		if (!a) return null;
		for (var d = 0; d < a.length; ++d) if ((a[d].ad_slot || b) == b && (a[d].ad_tag_origin || c) == c) return a[d];
		return null
	}
	function Ic(a) {
		V(this, a, lf)
	}
	function Jc(a) {
		V(this, a, null)
	}
	function Kc(a) {
		V(this, a, null)
	}
	function Lc(a) {
		V(this, a, mf)
	}
	function Mc(a) {
		V(this, a, nf)
	}
	function Nc(a) {
		V(this, a, null)
	}
	function Bb(a) {
		V(this, a, of)
	}
	function Cb(a) {
		V(this, a, null)
	}
	function Oc(a, b, c) {
		c = void 0 === c ? {} : c;
		this.error = a;
		this.context = b.context;
		this.line = b.line || -1;
		this.msg = b.message || "";
		this.file = b.file || "";
		this.id = b.id || "jserror";
		this.meta = c
	}
	function pf(a, b) {
		this.a = a;
		this.b = b
	}
	function Pc(a, b, c) {
		this.url = a;
		this.a = b;
		this.X = !! c;
		this.depth = D(void 0) ? void 0 : null
	}
	function Db() {
		this.g = "&";
		this.h = !1;
		this.b = {};
		this.i = 0;
		this.a = []
	}
	function Qc(a, b) {
		var c = {};
		c[a] = b;
		return [c]
	}
	function Rc(a, b, c, d, e) {
		var f = [];
		ub(a, function(a, h) {
			(a = Sc(a, b, c, d, e)) && f.push(h + "=" + a)
		});
		return f.join(b)
	}
	function Sc(a, b, c, d, e) {
		if (null == a) return "";
		b = b || "&";
		c = c || ",$";
		"string" == typeof c && (c = c.split(""));
		if (a instanceof Array) {
			if (d = d || 0, d < c.length) {
				for (var f = [], g = 0; g < a.length; g++) f.push(Sc(a[g], b, c, d + 1, e));
				return f.join(c[d])
			}
		} else if ("object" == typeof a) return e = e || 0, 2 > e ? encodeURIComponent(Rc(a, b, c, d, e + 1)) : "...";
		return encodeURIComponent(String(a))
	}
	function ua(a, b, c, d) {
		a.a.push(b);
		a.b[b] = Qc(c, d)
	}
	function qf(a, b, c, d) {
		b = b + "//" + c + d;
		var e = rf(a) - d.length;
		if (0 > e) return "";
		a.a.sort(function(a, b) {
			return a - b
		});
		d = null;
		c = "";
		for (var f = 0; f < a.a.length; f++) for (var g = a.a[f], h = a.b[g], k = 0; k < h.length; k++) {
			if (!e) {
				d = null == d ? g : d;
				break
			}
			var l = Rc(h[k], a.g, ",$");
			if (l) {
				l = c + l;
				if (e >= l.length) {
					e -= l.length;
					b += l;
					c = a.g;
					break
				} else a.h && (c = e, l[c - 1] == a.g && --c, b += l.substr(0, c), c = a.g, e = 0);
				d = null == d ? g : d
			}
		}
		a = "";
		null != d && (a = c + "trn=" + d);
		return b + a
	}
	function rf(a) {
		var b = 1,
			c;
		for (c in a.b) b = c.length > b ? c.length : b;
		return 3997 - b - a.g.length - 1
	}
	function Eb(a, b, c, d, e, f) {
		if ((d ? a.i : Math.random()) < (e || a.a)) try {
			if (c instanceof Db) var g = c;
			else g = new Db, ub(c, function(a, b) {
				var c = g,
					d = c.i++;
				a = Qc(b, a);
				c.a.push(d);
				c.b[d] = a
			});
			var h = qf(g, a.h, a.b, a.g + b + "&");
			h && ("undefined" === typeof f ? yb(h, void 0) : yb(h, f))
		} catch (k) {}
	}
	function Tc(a, b) {
		this.start = a < b ? a : b;
		this.a = a < b ? b : a
	}
	function ma(a, b) {
		this.a = b >= a ? new Tc(a, b) : null
	}
	function tf(a) {
		var b;
		try {
			a.localStorage && (b = parseInt(a.localStorage.getItem("google_experiment_mod"), 10))
		} catch (c) {
			return null
		}
		if (0 <= b && 1E3 > b) return b;
		if (Uc()) return null;
		b = Math.floor(1E3 * zc(a));
		try {
			if (a.localStorage) return a.localStorage.setItem("google_experiment_mod", "" + b), b
		} catch (c) {}
		return null
	}
	function Vc() {
		var a = l.performance;
		return a && a.now && a.timing ? Math.floor(a.now() + a.timing.navigationStart) : +new Date
	}
	function Wc() {
		var a = void 0 === a ? l : a;
		return (a = a.performance) && a.now ? a.now() : null
	}
	function uf(a, b, c) {
		this.label = a;
		this.type = b;
		this.value = c;
		this.duration = 0;
		this.uniqueId = this.label + "_" + this.type + "_" + Math.random();
		this.slotId = void 0
	}
	function Xc() {
		var a = Fa;
		this.b = [];
		this.g = a || l;
		var b = null;
		a && (a.google_js_reporting_queue = a.google_js_reporting_queue || [], this.b = a.google_js_reporting_queue, b = a.google_measure_js_timing);
		this.a = Ga() || (null != b ? b : 1 > Math.random())
	}
	function Yc(a) {
		a && O && Ga() && (O.clearMarks("goog_" + a.uniqueId + "_start"), O.clearMarks("goog_" + a.uniqueId + "_end"))
	}
	function Zc() {
		var a = Fb;
		this.w = $c;
		this.h = !0;
		this.g = null;
		this.A = this.a;
		this.b = void 0 === a ? null : a;
		this.i = !1
	}
	function ad(a, b, c, d, e) {
		try {
			if (a.b && a.b.a) {
				var f = a.b.start(b.toString(), 3),
					g = c(),
					h = a.b;
				c = f;
				if (h.a && D(c.value)) {
					var k = Wc() || Vc();
					c.duration = k - c.value;
					var l = "goog_" + c.uniqueId + "_end";
					O && Ga() && O.mark(l);
					h.a && h.b.push(c)
				}
			} else g = c()
		} catch (t) {
			h = a.h;
			try {
				Yc(f), h = (e || a.A).call(a, b, new Gb(Ha(t), t.fileName, t.lineNumber), void 0, d)
			} catch (sf) {
				a.a(217, sf)
			}
			if (!h) throw t;
		}
		return g
	}
	function bd(a, b) {
		var c = Y;
		return function(d) {
			for (var e = [], f = 0; f < arguments.length; ++f) e[f] = arguments[f];
			return ad(c, a, function() {
				return b.apply(void 0, e)
			}, void 0, void 0)
		}
	}
	function Ha(a) {
		var b = a.toString();
		a.name && -1 == b.indexOf(a.name) && (b += ": " + a.name);
		a.message && -1 == b.indexOf(a.message) && (b += ": " + a.message);
		if (a.stack) {
			a = a.stack;
			var c = b;
			try {
				-1 == a.indexOf(c) && (a = c + "\n" + a);
				for (var d; a != d;) d = a, a = a.replace(/((https?:\/..*\/)[^\/:]*:\d+(?:.|\n)*)\2/, "$1");
				b = a.replace(/\n */g, "\n")
			} catch (e) {
				b = c
			}
		}
		return b
	}
	function Gb(a, b, c) {
		Oc.call(this, Error(a), {
			message: a,
			file: void 0 === b ? "" : b,
			line: void 0 === c ? -1 : c
		})
	}
	function p(a) {
		a = void 0 === a ? "" : a;
		var b = Error.call(this);
		this.message = b.message;
		"stack" in b && (this.stack = b.stack);
		this.name = "TagError";
		this.message = a ? "adsbygoogle.push() error: " + a : "";
		Error.captureStackTrace ? Error.captureStackTrace(this, p) : this.stack = Error().stack || ""
	}
	function cd(a) {
		return 0 == (a.error && a.meta && a.id ? a.msg || Ha(a.error) : Ha(a)).indexOf("TagError")
	}

	function Hb(a) {
		null != a && (Fa.google_measure_js_timing = a);
		Fa.google_measure_js_timing || (a = Fb, a.a = !1, a.b != a.g.google_js_reporting_queue && (Ga() && kc(a.b, Yc), a.b.length = 0))
	}
	function vf() {
		var a = [wf, xf];
		Y.g = function(b) {
			kc(a, function(a) {
				a(b)
			})
		}
	}
	function na(a, b, c, d) {
		return ad(Y, a, c, d, b)
	}
	function Xa(a, b) {
		return bd(a, b)
	}
	function dd(a, b, c) {
		Eb($c, a, b, "jserror" != a, c, void 0)
	}
	function yf(a, b, c, d) {
		return cd(b) ? (Y.i = !0, Y.a(a, b, .1, d, "puberror"), !1) : Y.a(a, b, c, d)
	}
	function zf(a, b, c, d) {
		return cd(b) ? !1 : Y.a(a, b, c, d)
	}
	function Ib(a, b) {
		this.U = a;
		this.aa = b
	}
	function Af(a) {
		var b = [].slice.call(arguments).filter(ef());
		if (!b.length) return null;
		var c = [],
			d = {};
		b.forEach(function(a) {
			c = c.concat(a.U || []);
			d = Object.assign(d, a.aa)
		});
		return new Ib(c, d)
	}
	function Ya(a, b) {
		a.location.href && a.location.href.substring && (b.url = a.location.href.substring(0, 200));
		dd("ama", b, .01)
	}
	function Za(a, b, c) {
		c || (c = Bf ? "https" : "http");
		l.location && "https:" == l.location.protocol && "http" == c && (c = "https");
		return [c, "://", a, b].join("")
	}
	function Cf() {
		if (!da) return !1;
		if (null != va) return va;
		va = !1;
		try {
			var a = X(l);
			a && -1 != a.location.hash.indexOf("google_logging") && (va = !0);
			l.localStorage.getItem("google_logging") && (va = !0)
		} catch (b) {}
		return va
	}
	function $a(a, b) {
		b = void 0 === b ? [] : b;
		if (Cf()) {
			if (!l.google_logging_queue) {
				l.google_logging_queue = [];
				var c = Za(Ec(), "/pagead/js/logging_library.js");
				tb(l.document, c)
			}
			l.google_logging_queue.push([a, b])
		}
	}
	function ed(a) {
		V(this, a, null)
	}
	function fd() {
		if (!Jb) {
			for (var a = l, b = a, c = 0; a && a != a.parent;) if (a = a.parent, c++, M(a)) b = a;
			else break;
			Jb = b
		}
		return Jb
	}

	function Df() {
		var a = m.google_page_location || m.google_page_url;
		"EMPTY" == a && (a = m.google_page_url);
		if (da || !a) return !1;
		a = a.toString();
		0 == a.indexOf("http://") ? a = a.substring(7, a.length) : 0 == a.indexOf("https://") && (a = a.substring(8, a.length));
		var b = a.indexOf("/"); - 1 == b && (b = a.length);
		a = a.substring(0, b);
		if (Ef.test(a)) return !1;
		a = a.split(".");
		b = !1;
		3 <= a.length && (b = a[a.length - 3] in gd);
		2 <= a.length && (b = b || a[a.length - 2] in gd);
		return b
	}
	function ab(a) {
		this.a = this.b = null;
		"function" == ba(a) ? this.a = a : this.b = a
	}
	function Ff() {
		this.wasPlaTagProcessed = !1;
		this.wasReactiveAdConfigReceived = {};
		this.adCount = {};
		this.wasReactiveAdVisible = {};
		this.stateForType = {};
		this.reactiveTypeEnabledInAsfe = {};
		this.isReactiveTagFirstOnPage = this.wasReactiveAdConfigHandlerRegistered = this.wasReactiveTagRequestSent = !1;
		this.reactiveTypeDisabledByPublisher = {};
		this.tagSpecificState = {};
		this.messageValidationEnabled = !1;
		this.adRegion = this.floatingAdsFillMessage = null;
		this.disablePageHeightCheck = !1
	}
	function hd(a) {
		a.google_reactive_ads_global_state || (a.google_reactive_ads_global_state = new Ff);
		return a.google_reactive_ads_global_state
	}
	function bb(a) {
		a = a.document;
		var b = {};
		a && (b = "CSS1Compat" == a.compatMode ? a.documentElement : a.body);
		return b || {}
	}
	function I(a) {
		return bb(a).clientWidth
	}
	function id(a, b) {
		for (var c = [], d = a.length, e = 0; e < d; e++) c.push(a[e]);
		c.forEach(b, void 0)
	}
	function Kb(a, b, c, d) {
		this.h = a;
		this.b = b;
		this.g = c;
		this.a = d
	}
	function Gf(a, b) {
		if (null == a.a) return b;
		switch (a.a) {
		case 1:
			return b.slice(1);
		case 2:
			return b.slice(0, b.length - 1);
		case 3:
			return b.slice(1, b.length - 1);
		case 0:
			return b;
		default:
			throw Error("Unknown ignore mode: " + a.a);
		}
	}
	function Hf(a) {
		var b = [];
		id(a.getElementsByTagName("p"), function(a) {
			100 <= jd(a) && b.push(a)
		});
		return b
	}
	function jd(a) {
		if (3 == a.nodeType) return a.length;
		if (1 != a.nodeType || "SCRIPT" == a.tagName) return 0;
		var b = 0;
		id(a.childNodes, function(a) {
			b += jd(a)
		});
		return b
	}
	function kd(a) {
		return 0 == a.length || isNaN(a[0]) ? a : "\\" + (30 + parseInt(a[0], 10)) + " " + a.substring(1)
	}
	function ld(a, b) {
		for (var c = ["width", "height"], d = 0; d < c.length; d++) {
			var e = "google_ad_" + c[d];
			if (!b.hasOwnProperty(e)) {
				var f = A(a[c[d]]);
				f = null === f ? null : Math.round(f);
				null != f && (b[e] = f)
			}
		}
	}
	function md(a, b) {
		return !((wb.test(b.google_ad_width) || xb.test(a.style.width)) && (wb.test(b.google_ad_height) || xb.test(a.style.height)))
	}
	function nd(a, b) {
		try {
			var c = b.document.documentElement.getBoundingClientRect(),
				d = a.getBoundingClientRect(),
				e = {
					x: d.left - c.left,
					y: d.top - c.top
				}
		} catch (f) {
			e = null
		}
		return (a = e) ? a.y : 0
	}
	function od(a, b) {
		do {
			var c = N(a, b);
			if (c && "fixed" == c.position) return !1
		} while (a = a.parentElement);
		return !0
	}
	function pd(a) {
		var b = 0,
			c;
		for (c in qd) - 1 != a.indexOf(c) && (b |= qd[c]);
		return b
	}
	function Lb(a, b, c, d, e) {
		if ((da && a.google_top_window || a.top) != a) return X(a) ? 3 : 16;
		if (!(488 > I(a))) return 4;
		if (!(a.innerHeight >= a.innerWidth)) return 5;
		var f = I(a);
		if (!f || (f - c) / f > d) a = 6;
		else {
			if (c = "true" != e.google_full_width_responsive) a: {
				c = I(a);
				b = b.parentElement;
				for (d = 0; 100 > d && b; d++) {
					if ((e = N(b, a)) && "hidden" == e.overflowX && (e = A(e.width)) && e < c) {
						c = !0;
						break a
					}
					b = b.parentElement
				}
				c = !1
			}
			a = c ? 7 : !0
		}
		return a
	}
	function Mb(a, b, c, d) {
		var e = Lb(b, c, a, .3, d);
		if (!0 !== e) return e;
		e = I(b);
		a = e - a;
		a = e && 0 <= a ? !0 : e ? -10 > a ? 11 : 0 > a ? 14 : 12 : 10;
		return "true" == d.google_full_width_responsive || od(c, b) ? a : 9
	}
	function If(a, b) {
		if (3 == b.nodeType) return /\S/.test(b.data);
		if (1 == b.nodeType) {
			if (/^(script|style)$/i.test(b.nodeName)) return !1;
			try {
				var c = N(b, a)
			} catch (d) {}
			return !c || "none" != c.display && !("absolute" == c.position && ("hidden" == c.visibility || "collapse" == c.visibility))
		}
		return !1
	}
	function Ia(a, b, c, d, e, f) {
		if (a = N(c, a)) {
			var g = A(a.paddingLeft) || 0;
			a = a.direction;
			d = e - d;
			if (f.google_ad_resize) g = -1 * (d + g) + "px";
			else {
				for (var h = f = 0; 100 > h && c; h++) f += c.offsetLeft + c.clientLeft - c.scrollLeft, c = c.offsetParent;
				g = f + g;
				g = "rtl" == a ? -1 * (d - g) + "px" : -1 * g + "px"
			}
			"rtl" == a ? b.style.marginRight = g : b.style.marginLeft = g;
			b.style.width = e + "px";
			b.style.zIndex = 30
		}
	}
	function C(a, b) {
		this.b = a;
		this.a = b
	}
	function J(a, b, c, d) {
		d = void 0 === d ?
		function(a) {
			return a
		} : d;
		var e;
		return a.style && a.style[c] && d(a.style[c]) || (e = N(a, b)) && e[c] && d(e[c]) || null
	}
	function rd(a) {
		return function(b) {
			return b.minWidth() <= a
		}
	}
	function Jf(a, b, c, d) {
		var e = a && Nb(c, b),
			f = Kf(b, d);
		return function(a) {
			return !(e && a.height() >= f)
		}
	}
	function sd(a) {
		return function(b) {
			return b.height() <= a
		}
	}
	function Nb(a, b) {
		return nd(a, b) < bb(b).clientHeight - 100
	}
	function td(a, b) {
		var c = Infinity;
		do {
			var d = J(b, a, "height", A);
			d && (c = Math.min(c, d));
			(d = J(b, a, "maxHeight", A)) && (c = Math.min(c, d))
		} while ((b = b.parentElement) && "HTML" != b.tagName);
		return c
	}
	function ud(a, b) {
		var c = J(b, a, "height", A);
		if (c) return c;
		var d = b.style.height;
		b.style.height = "inherit";
		c = J(b, a, "height", A);
		b.style.height = d;
		if (c) return c;
		c = Infinity;
		do(d = b.style && A(b.style.height)) && (c = Math.min(c, d)), (d = J(b, a, "maxHeight", A)) && (c = Math.min(c, d));
		while ((b = b.parentElement) && "HTML" != b.tagName);
		return c
	}
	function Kf(a, b) {
		var c = a.google_unique_id;
		return b && 0 == ("number" === typeof c ? c : 0) ? Math.max(250, 2 * bb(a).clientHeight / 3) : 250
	}
	function Ob(a) {
		if (1 != a.nodeType) var b = !1;
		else if (b = "INS" == a.tagName) a: {
			b = ["adsbygoogle-placeholder"];
			a = a.className ? a.className.split(/\s+/) : [];
			for (var c = {}, d = 0; d < a.length; ++d) c[a[d]] = !0;
			for (d = 0; d < b.length; ++d) if (!c[b[d]]) {
				b = !1;
				break a
			}
			b = !0
		}
		return b
	}
	function vd(a, b) {
		for (var c = 0; c < b.length; c++) {
			var d = b[c],
				e = af(d.Qa);
			a[e] = d.value
		}
	}
	function wd(a) {
		return null != a ? Lf[a] : a
	}
	function Pb(a, b) {
		if (!a) return !1;
		a = N(a, b);
		if (!a) return !1;
		a = a.cssFloat || a.styleFloat;
		return "left" == a || "right" == a
	}
	function xd(a) {
		for (a = a.previousSibling; a && 1 != a.nodeType;) a = a.previousSibling;
		return a ? a : null
	}
	function yd(a) {
		return !!a.nextSibling || !! a.parentNode && yd(a.parentNode)
	}
	function Mf(a, b, c) {
		this.a = l;
		this.i = a;
		this.h = b;
		this.g = c || null;
		this.b = !1
	}
	function zd(a, b) {
		if (a.b) return !0;
		try {
			var c = a.a.localStorage.getItem("google_ama_settings"),
				d = c ? new ed(c ? JSON.parse(c) : null) : null
		} catch (h) {
			d = null
		}
		if (null !== d && nc(d, 2)) return a = a.a.google_ama_state = a.a.google_ama_state || {}, a.eatf = !0, $a(7, [!0, 0, !1]), !0;
		d = 0;
		var e = oc(a.h, Lc, 1);
		for (c = 0; c < e.length; c++) {
			var f = e[c];
			if (1 == v(f, 8)) {
				var g = qa(f, Kc, 4);
				if (g && 2 == v(g, 1) && (d++, Nf(a, f, b))) return a.b = !0, a = a.a.google_ama_state = a.a.google_ama_state || {}, a.placement = c, $a(7, [!1, d, !0]), !0
			}
		}
		$a(7, [!1, d, !1]);
		return !1
	}
	function Nf(a, b, c) {
		if (1 != v(b, 8)) return !1;
		var d = qa(b, Ic, 1);
		if (!d) return !1;
		var e = v(d, 7);
		if (v(d, 1) || v(d, 3) || 0 < Aa(d, 4).length) {
			var f = v(d, 3),
				g = v(d, 1),
				h = Aa(d, 4);
			e = v(d, 2);
			var k = v(d, 5);
			d = wd(v(d, 6));
			var l = "";
			g && (l += g);
			f && (l += "#" + kd(f));
			if (h) for (f = 0; f < h.length; f++) l += "." + kd(h[f]);
			e = (h = l) ? new Kb(h, e, k, d) : null
		} else e = e ? new Kb(e, v(d, 2), v(d, 5), wd(v(d, 6))) : null;
		if (!e) return !1;
		k = [];
		try {
			k = a.a.document.querySelectorAll(e.h)
		} catch (r) {}
		if (k.length) {
			h = k.length;
			if (0 < h) {
				d = Array(h);
				for (f = 0; f < h; f++) d[f] = k[f];
				k = d
			} else k = [];
			k = Gf(e, k);
			D(e.b) && (h = e.b, 0 > h && (h += k.length), k = 0 <= h && h < k.length ? [k[h]] : []);
			if (D(e.g)) {
				h = [];
				for (d = 0; d < k.length; d++) f = Hf(k[d]), g = e.g, 0 > g && (g += f.length), 0 <= g && g < f.length && h.push(f[g]);
				k = h
			}
			e = k
		} else e = [];
		if (0 == e.length) return !1;
		e = e[0];
		k = v(b, 2);
		k = Of[k];
		k = void 0 !== k ? k : null;
		if (!(h = null == k)) {
			a: {
				h = a.a;
				switch (k) {
				case 0:
					h = Pb(xd(e), h);
					break a;
				case 3:
					h = Pb(e, h);
					break a;
				case 2:
					d = e.lastChild;
					h = Pb(d ? 1 == d.nodeType ? d : xd(d) : null, h);
					break a
				}
				h = !1
			}
			if (c = !h && !(!c && 2 == k && !yd(e))) c = 1 == k || 2 == k ? e : e.parentNode,
			c = !(c && !Ob(c) && 0 >= c.offsetWidth);
			h = !c
		}
		if (h) return !1;
		b = qa(b, Jc, 3);
		h = {};
		b && (h.ga = v(b, 1), h.R = v(b, 2), h.ra = !! v(b, 3));
		b = a.a;
		c = a.g;
		d = a.i;
		f = b.document;
		a = Ua(new Ba(f), "DIV");
		g = a.style;
		g.textAlign = "center";
		g.width = "100%";
		g.height = "auto";
		g.clear = h.ra ? "both" : "none";
		h.xa && vd(g, h.xa);
		f = Ua(new Ba(f), "INS");
		g = f.style;
		g.display = "block";
		g.margin = "auto";
		g.backgroundColor = "transparent";
		h.ga && (g.marginTop = h.ga);
		h.R && (g.marginBottom = h.R);
		h.qa && vd(g, h.qa);
		a.appendChild(f);
		f.setAttribute("data-ad-format", "auto");
		h = [];
		if (g = c && c.U) a.className = g.join(" ");
		f.className = "adsbygoogle";
		f.setAttribute("data-ad-client", d);
		h.length && f.setAttribute("data-ad-channel", h.join("+"));
		a: {
			try {
				switch (k) {
				case 0:
					e.parentNode && e.parentNode.insertBefore(a, e);
					break;
				case 3:
					var t = e.parentNode;
					if (t) {
						var m = e.nextSibling;
						if (m && m.parentNode != t) for (; m && 8 == m.nodeType;) m = m.nextSibling;
						t.insertBefore(a, m)
					}
					break;
				case 1:
					e.insertBefore(a, e.firstChild);
					break;
				case 2:
					e.appendChild(a)
				}
				Ob(e) && (e.setAttribute("data-init-display", e.style.display), e.style.display = "block");
				b: {
					f.setAttribute("data-adsbygoogle-status", "reserved");
					f.className += " adsbygoogle-noablate";
					t = {
						element: f
					};
					var n = c && c.aa;
					if (f.hasAttribute("data-pub-vars")) {
						try {
							n = JSON.parse(f.getAttribute("data-pub-vars"))
						} catch (r) {
							break b
						}
						f.removeAttribute("data-pub-vars")
					}
					n && (t.params = n);
					(b.adsbygoogle = b.adsbygoogle || []).push(t)
				}
			} catch (r) {
				a && a.parentNode && (n = a.parentNode, n.removeChild(a), Ob(n) && (n.style.display = n.getAttribute("data-init-display") || "none"));
				n = !1;
				break a
			}
			n = !0
		}
		return n ? !0 : !1
	}
	function Pf() {
		this.b = new Ad(this);
		this.a = 0
	}
	function Bd(a) {
		if (0 != a.a) throw Error("Already resolved/rejected.");
	}
	function Ad(a) {
		this.a = a
	}
	function Qb(a) {
		switch (a.a.a) {
		case 0:
			break;
		case 1:
			a.b && a.b(a.a.h);
			break;
		case 2:
			a.g && a.g(a.a.g);
			break;
		default:
			throw Error("Unhandled deferred state.");
		}
	}
	function Qf(a) {
		this.exception = a
	}
	function Rb(a, b) {
		this.b = l;
		this.g = a;
		this.a = b
	}
	function Sb(a, b) {
		try {
			var c = a.a,
				d = new Qf(b);
			Bd(c);
			c.a = 1;
			c.h = d;
			Qb(c.b)
		} catch (e) {
			a = a.a, b = e, Bd(a), a.a = 2, a.g = b, Qb(a.b)
		}
	}
	function Rf(a) {
		Ya(a, {
			atf: 1
		})
	}
	function Sf(a, b) {
		(a.google_ama_state = a.google_ama_state || {}).exception = b;
		Ya(a, {
			atf: 0
		})
	}
	function Tf() {
		this.debugCard = null;
		this.debugCardRequested = !1
	}
	function xf(a) {
		try {
			var b = l.google_ad_modifications;
			if (null != b) {
				var c = Te(b.eids, b.loeids);
				null != c && 0 < c.length && (a.eid = c.join(","))
			}
		} catch (d) {}
	}
	function wf(a) {
		a.shv = "r20180924"
	}
	function Ja(a, b) {
		if (!a) return !1;
		a = a.hash;
		if (!a || !a.indexOf) return !1;
		if (-1 != a.indexOf(b)) return !0;
		b = Uf(b);
		return "go" != b && -1 != a.indexOf(b) ? !0 : !1
	}
	function Uf(a) {
		var b = "";
		ca(a.split("_"), function(a) {
			b += a.substr(0, 2)
		});
		return b
	}
	function Vf() {
		this.a = Cd
	}
	function n(a, b) {
		a = parseFloat(a.a[b]);
		return isNaN(a) ? 0 : a
	}
	function Tb() {
		Ub || (Ub = new Vf);
		return Ub
	}
	function Vb(a, b, c, d, e) {
		d = void 0 === d ? "" : d;
		var f = a.createElement("link");
		try {
			f.rel = c;
			if (E(c.toLowerCase(), "stylesheet")) var g = rb(b);
			else {
				if (b instanceof ra) var h = rb(b);
				else {
					if (b instanceof ka) if (b instanceof ka && b.constructor === ka && b.oa === sc) var k = b.I;
					else ba(b), k = "type_error:SafeUrl";
					else {
						if (b instanceof ka) var l = b;
						else b = "object" == typeof b && b.g ? b.a() : String(b), Wf.test(b) || (b = "about:invalid#zClosurez"), l = tc(b);
						k = l.a()
					}
					h = k
				}
				g = h
			}
			f.href = g
		} catch (t) {
			return
		}
		d && "preload" == c && (f.as = d);
		e && (f.nonce = e);
		if (a = a.getElementsByTagName("head")[0]) try {
			a.appendChild(f)
		} catch (t) {}
	}

	function Dd(a, b) {
		function c(a) {
			a ? (a = String(a).split(","), a = 0 <= Ta(a, "11")) : a = !1;
			return a
		}
		b = void 0 === b ? "" : b;
		a = X(a) || a;
		try {
			var d = Xf({}, JSON.parse(a.localStorage.getItem("google_adsense_labs")))
		} catch (e) {
			d = {}
		}
		return Yf(a) ? !0 : b ? c(d[b]) : bf(d, c)
	}
	function Yf(a) {
		a = (a = (a = a.location && a.location.hash) && a.match(/forced_clientside_labs=([\d,]+)/)) && a[1];
		return !!a && 0 <= Ta(a.split(","), (11).toString())
	}
	function q(a, b) {
		b && a.push(b)
	}
	function cb(a, b) {
		for (var c = [], d = 1; d < arguments.length; ++d) c[d - 1] = arguments[d];
		d = X(a) || a;
		d = (d = (d = d.location && d.location.hash) && (d.match(/google_plle=([\d,]+)/) || d.match(/deid=([\d,]+)/))) && d[1];
		var e;
		if (e = !! d) a: {
			d = Sa(E, d);
			e = c.length;
			for (var f = aa(c) ? c.split("") : c, g = 0; g < e; g++) if (g in f && d.call(void 0, f[g], g, c)) {
				e = !0;
				break a
			}
			e = !1
		}
		return e
	}
	function x(a, b, c) {
		for (var d = 0; d < c.length; d++) if (cb(a, c[d])) return c[d];
		!Uc() && (a = Math.random(), a < b) ? (a = zc(l), b = c[Math.floor(a * c.length)]) : b = null;
		return b
	}
	function oa(a, b, c, d, e, f) {
		f = void 0 === f ? 1 : f;
		for (var g = 0; g < e.length; g++) if (cb(a, e[g])) return e[g];
		f = void 0 === f ? 1 : f;
		0 >= d ? c = null : (g = new Tc(c, c + d - 1), (d = d % f || d / f % e.length) || (b = b.a, d = !(b.start <= g.start && b.a >= g.a)), d ? c = null : (a = tf(a), c = null !== a && g.start <= a && g.a >= a ? e[Math.floor((a - c) / f) % e.length] : null));
		return c
	}
	function Ed(a) {
		if (!a) return "";
		(a = a.toLowerCase()) && "ca-" != a.substring(0, 3) && (a = "ca-" + a);
		return a
	}
	function Fd(a, b, c, d) {
		d = void 0 === d ? "" : d;
		var e = ["<iframe"],
			f;
		for (f in a) a.hasOwnProperty(f) && e.push(f + "=" + a[f]);
		e.push('style="' + ("left:0;position:absolute;top:0;width:" + b + "px;height:" + c + "px;") + '"');
		e.push("></iframe>");
		a = a.id;
		b = "border:none;height:" + c + "px;margin:0;padding:0;position:relative;visibility:visible;width:" + b + "px;background-color:transparent;";
		return ['<ins id="', a + "_expand", '" style="display:inline-table;', b, void 0 === d ? "" : d, '"><ins id="', a + "_anchor", '" style="display:block;', b, '">', e.join(" "), "</ins></ins>"].join("")
	}
	function Gd(a, b, c) {
		if (M(a.document.getElementById(b).contentWindow)) a = a.document.getElementById(b).contentWindow, b = a.document, b.body && b.body.firstChild || (/Firefox/.test(navigator.userAgent) ? b.open("text/html", "replace") : b.open(), a.google_async_iframe_close = !0, b.write(c));
		else {
			a = a.document.getElementById(b).contentWindow;
			c = String(c);
			b = ['"'];
			for (var d = 0; d < c.length; d++) {
				var e = c.charAt(d),
					f = e.charCodeAt(0),
					g = d + 1,
					h;
				if (!(h = Wb[e])) {
					if (!(31 < f && 127 > f)) if (f = e, f in db) e = db[f];
					else if (f in Wb) e = db[f] = Wb[f];
					else {
						h = f.charCodeAt(0);
						if (31 < h && 127 > h) e = f;
						else {
							if (256 > h) {
								if (e = "\\x", 16 > h || 256 < h) e += "0"
							} else e = "\\u", 4096 > h && (e += "0");
							e += h.toString(16).toUpperCase()
						}
						e = db[f] = e
					}
					h = e
				}
				b[g] = h
			}
			b.push('"');
			a.location.replace("javascript:" + b.join(""))
		}
	}
	function w(a, b, c, d) {
		d = void 0 === d ? !1 : d;
		C.call(this, a, b);
		this.B = c;
		this.va = d
	}
	function Zf(a) {
		return function(b) {
			return !!(b.B & a)
		}
	}
	function K(a, b, c, d, e, f, g, h, k, l, t) {
		this.Ha = a;
		this.a = b;
		this.B = void 0 === c ? null : c;
		this.ea = void 0 === d ? null : d;
		this.b = void 0 === e ? null : e;
		this.g = void 0 === f ? null : f;
		this.Y = void 0 === g ? null : g;
		this.Z = void 0 === h ? null : h;
		this.h = void 0 === k ? null : k;
		this.i = void 0 === l ? null : l;
		this.$ = void 0 === t ? null : t;
		this.ha = this.A = this.w = null
	}
	function Xb(a, b, c) {
		null != a.B && (c.google_responsive_formats = a.B);
		null != a.ea && (c.google_safe_for_responsive_override = a.ea);
		null != a.b && (!0 === a.b ? c.google_full_width_responsive_allowed = !0 : (c.google_full_width_responsive_allowed = !1, c.gfwrnwer = a.b));
		null != a.g && !0 !== a.g && (c.gfwrnher = a.g);
		var d = a.a.s(b),
			e = a.a.height();
		1 != c.google_ad_resize && (c.google_ad_width = d, c.google_ad_height = e, c.google_ad_format = a.a.G(b), c.google_responsive_auto_format = a.Ha, c.google_ad_resizable = !0, c.google_override_format = 1, c.google_loader_features_used = 128, !0 === a.b && (c.gfwrnh = a.a.height() + "px"));
		null != a.Y && (c.gfwroml = a.Y);
		null != a.Z && (c.gfwromr = a.Z);
		null != a.h && (c.gfwroh = a.h, c.google_resizing_height = a.h);
		null != a.i && (c.gfwrow = a.i, c.google_resizing_width = a.i);
		null != a.$ && (c.gfwroz = a.$);
		null != a.w && (c.gml = a.w);
		null != a.A && (c.gmr = a.A);
		null != a.ha && (c.gzi = a.ha);
		b = sa();
		b = X(b) || b;
		if (Ja(b.location, "google_responsive_slot_debug") || cb(b, ea.c, ea.f)) c.ds = "outline:thick dashed " + (d && e ? !0 !== a.b || !0 !== a.g ? "#ffa500" : "#0f0" : "#f00") + " !important;"
	}
	function Z(a, b) {
		C.call(this, a, b)
	}
	function $f(a) {
		var b = 0;
		ca(Hd, function(c) {
			null != a[c] && ++b
		});
		if (0 === b) return !1;
		if (b === Hd.length) return !0;
		throw new p("Tags data-matched-content-ui-type, data-matched-content-columns-num and data-matched-content-rows-num should be set together.");
	}
	function ag(a, b) {
		Id(a, b);
		if (a < Jd) {
			if (sb()) {
				var c = b;
				c.google_content_recommendation_ui_type = "mobile_banner_image_sidebyside";
				c.google_content_recommendation_columns_num = 1;
				c.google_content_recommendation_rows_num = 12;
				c = +b.google_content_recommendation_columns_num;
				c = (a - 8 * c - 8) / c;
				var d = b.google_content_recommendation_ui_type;
				b = b.google_content_recommendation_rows_num - 1;
				return new K(9, new Z(a, Math.floor(c / 1.91 + 70) + Math.floor((c * Kd[d] + Ld[d]) * b + 8 * b + 8)))
			}
			b.google_content_recommendation_ui_type = "image_sidebyside";
			b.google_content_recommendation_columns_num = 1;
			b.google_content_recommendation_rows_num = 13;
			return new K(9, Md(a))
		}
		b.google_content_recommendation_ui_type = "image_stacked";
		b.google_content_recommendation_columns_num = 4;
		b.google_content_recommendation_rows_num = 2;
		return new K(9, Md(a))
	}
	function Md(a) {
		return 1200 <= a ? new Z(1200, 600) : 850 <= a ? new Z(a, Math.floor(.5 * a)) : 550 <= a ? new Z(a, Math.floor(.6 * a)) : 468 <= a ? new Z(a, Math.floor(.7 * a)) : new Z(a, Math.floor(3.44 * a))
	}
	function bg(a, b) {
		Id(a, b);
		var c = b.google_content_recommendation_ui_type.split(","),
			d = b.google_content_recommendation_columns_num.split(","),
			e = b.google_content_recommendation_rows_num.split(",");
		a: {
			if (c.length == d.length && d.length == e.length) {
				if (1 == c.length) {
					var f = 0;
					break a
				}
				if (2 == c.length) {
					f = a < Jd ? 0 : 1;
					break a
				}
				throw new p("The parameter length of attribute data-matched-content-ui-type, data-matched-content-columns-num and data-matched-content-rows-num is too long. At most 2 parameters for each attribute are needed: one for mobile and one for desktop, while " + ("you are providing " + c.length + ' parameters. Example: \n data-matched-content-rows-num="4,2"\ndata-matched-content-columns-num="1,6"\ndata-matched-content-ui-type="image_stacked,image_card_sidebyside".'));
			}
			if (c.length != d.length) throw new p('The parameter length of data-matched-content-ui-type does not match data-matched-content-columns-num. Example: \n data-matched-content-rows-num="4,2"\ndata-matched-content-columns-num="1,6"\ndata-matched-content-ui-type="image_stacked,image_card_sidebyside".');
			throw new p('The parameter length of data-matched-content-columns-num does not match data-matched-content-rows-num. Example: \n data-matched-content-rows-num="4,2"\ndata-matched-content-columns-num="1,6"\ndata-matched-content-ui-type="image_stacked,image_card_sidebyside".');
		}
		c = c[f];
		c = 0 == c.lastIndexOf("pub_control_", 0) ? c : "pub_control_" + c;
		d = +d[f];
		for (var g = cg[c], h = d; a / h < g && 1 < h;) h--;
		h !== d && l.console && l.console.warn("adsbygoogle warning: data-matched-content-columns-num " + d + " is too large. We override it to " + h + ".");
		d = h;
		e = +e[f];
		b.google_content_recommendation_ui_type = c;
		b.google_content_recommendation_columns_num = d;
		b.google_content_recommendation_rows_num = e;
		if (Number.isNaN(d) || 0 === d) throw new p("Wrong value for data-matched-content-columns-num");
		if (Number.isNaN(e) || 0 === e) throw new p("Wrong value for data-matched-content-rows-num");
		b = Math.floor(((a - 8 * d - 8) / d * Kd[c] + Ld[c]) * e + 8 * e + 8);
		if (1500 < a) throw new p("Calculated slot width is too large: " + a);
		if (1500 < b) throw new p("Calculated slot height is too large: " + b);
		return new K(9, new Z(a, b))
	}
	function Id(a, b) {
		if (0 >= a) throw new p("Invalid responsive width from Matched Content slot " + b.google_ad_slot + ": " + a + ". Please ensure to put this Matched Content slot into a non-zero width div container.");
	}
	function wa(a, b) {
		C.call(this, a, b)
	}
	function Nd(a) {
		return function(b) {
			for (var c = a.length - 1; 0 <= c; --c) if (!a[c](b)) return !1;
			return !0
		}
	}
	function Od(a, b, c) {
		for (var d = a.length, e = null, f = 0; f < d; ++f) {
			var g = a[f];
			if (b(g)) {
				if (!c || c(g)) return g;
				null === e && (e = g)
			}
		}
		return e
	}
	function Pd(a, b, c, d, e) {
		"false" != e.google_full_width_responsive || c.location && "#gfwrffwaifhp" == c.location.hash ? "auto" == b || 0 < (pd(b) & 1) || "autorelaxed" == b && Va(Wa(c), Qd.f) || Rd(b) || 1 == e.google_ad_resize ? (b = Mb(a, c, d, e), c = !0 !== b ? {
			j: a,
			l: b
		} : {
			j: I(c) || a,
			l: !0
		}) : c = {
			j: a,
			l: 2
		} : c = {
			j: a,
			l: 1
		};
		b = c.l;
		return !0 !== b ? {
			j: a,
			l: b
		} : d.parentElement ? {
			j: c.j,
			l: b
		} : {
			j: a,
			l: b
		}
	}
	function Yb(a, b, c, d, e) {
		var f = na(247, Ka, function() {
			return Pd(a, b, c, d, e)
		}),
			g = f.j;
		f = f.l;
		var h = !0 === f,
			k = J(d, c, "width", A) || e.google_ad_width || 0,
			l = J(d, c, "height", A) || e.google_ad_height || 0,
			t = Sd(g, b, c, d, e, h);
		g = t.v;
		h = t.u;
		var m = t.J;
		t = t.wa;
		var n = dg(b, m),
			r, q = (r = J(d, c, "marginLeft", A)) ? r + "px" : "",
			p = (r = J(d, c, "marginRight", A)) ? r + "px" : "";
		r = J(d, c, "zIndex") || "";
		return new K(n, g, m, t, f, h, q, p, l, k, r)
	}
	function Rd(a) {
		return "auto" == a || /^((^|,) *(horizontal|vertical|rectangle) *)+$/.test(a)
	}
	function Sd(a, b, c, d, e, f) {
		b = "auto" == b ? .25 >= a / Math.min(1200, I(c)) ? 4 : 3 : pd(b);
		var g = !1,
			h = !1;
		if (sb()) {
			var k = od(d, c),
				l = Nb(d, c);
			g = !l && k;
			h = l && k
		}
		l = (g ? eg : z).slice(0);
		var t = 488 > I(c);
		t = [rd(a), fg(t), Jf(t, c, d, h), Zf(b)];
		null != e.google_max_responsive_height && t.push(sd(e.google_max_responsive_height));
		var m = [function(a) {
			return !a.va
		}];
		if (g || h) g = g ? td(c, d) : ud(c, d), m.push(sd(g));
		var n = Od(l, Nd(t), Nd(m));
		if (!n) throw new p("No slot size for availableWidth=" + a);
		g = na(248, Ka, function() {
			var b;
			if (f) if (e.gfwrnh && (b = A(e.gfwrnh))) b = {
				v: new wa(a, b),
				u: !0
			};
			else if ("true" != e.google_full_width_responsive && Nb(d, c) && !e.google_resizing_allowed) b = {
				v: new wa(a, n.height()),
				u: 101
			};
			else {
				b = a / 1.2;
				var g = e.google_resizing_allowed || "true" == e.google_full_width_responsive ? Infinity : td(c, d);
				g = Math.min(b, g);
				if (g < .5 * b || 100 > g) g = b;
				b = {
					v: new wa(a, Math.floor(g)),
					u: g < b ? 102 : !0
				}
			} else b = {
				v: n,
				u: 100
			};
			return b
		});
		return {
			v: g.v,
			u: g.u,
			J: b,
			wa: k
		}
	}
	function dg(a, b) {
		if ("auto" == a) return 1;
		switch (b) {
		case 2:
			return 2;
		case 1:
			return 3;
		case 4:
			return 4;
		case 3:
			return 5;
		case 6:
			return 6;
		case 5:
			return 7;
		case 7:
			return 8
		}
		throw Error("bad mask");
	}
	function fg(a) {
		return function(b) {
			return !(320 == b.minWidth() && (a && 50 == b.height() || !a && 100 == b.height()))
		}
	}
	function Zb(a, b) {
		C.call(this, a, b)
	}
	function gg(a, b, c, d, e) {
		var f = e.google_ad_layout || "image-top";
		if ("in-article" == f && "false" != e.google_full_width_responsive) {
			var g = Lb(b, c, a, .2, e);
			if (!0 !== g) e.gfwrnwer = g;
			else if (g = I(b)) {
				e.google_full_width_responsive_allowed = !0;
				var h = c.parentElement;
				if (h) {
					var k = c,
						l = 0;
					a: for (; 100 > l && k.parentElement; ++l) {
						for (var t = k.parentElement.childNodes, m = 0; m < t.length; ++m) {
							var n = t[m];
							if (n != k && If(b, n)) break a
						}
						k = k.parentElement;
						k.style.width = "100%";
						k.style.height = "auto"
					}
					Ia(b, c, h, a, g, e);
					a = g
				}
			}
		}
		if (250 > a) throw new p("Fluid responsive ads must be at least 250px wide: availableWidth=" + a);
		b = Math.min(1200, Math.floor(a));
		if (d && "in-article" != f) {
			f = Math.ceil(d);
			if (50 > f) throw new p("Fluid responsive ads must be at least 50px tall: height=" + f);
			return new K(11, new C(b, f))
		}
		if ("in-article" != f && (d = e.google_ad_layout_key)) {
			f = "" + d;
			d = Math.pow(10, 3);
			if (c = (e = f.match(/([+-][0-9a-z]+)/g)) && e.length) {
				a = [];
				for (g = 0; g < c; g++) a.push(parseInt(e[g], 36) / d);
				d = a
			} else d = null;
			if (!d) throw new p("Invalid data-ad-layout-key value: " + f);
			f = (b + -725) / 1E3;
			e = 0;
			c = 1;
			a = d.length;
			for (g = 0; g < a; g++) e += d[g] * c, c *= f;
			f = Math.ceil(1E3 * e - -725 + 10);
			if (isNaN(f)) throw new p("Invalid height: height=" + f);
			if (50 > f) throw new p("Fluid responsive ads must be at least 50px tall: height=" + f);
			if (1200 < f) throw new p("Fluid responsive ads must be at most 1200px tall: height=" + f);
			return new K(11, new C(b, f))
		}
		d = hg[f];
		if (!d) throw new p("Invalid data-ad-layout value: " + f);
		d = Math.ceil(d(b));
		return new K(11, "in-article" == f ? new Zb(b, d) : new C(b, d))
	}
	function P(a, b) {
		C.call(this, a, b)
	}
	function ig(a, b, c, d) {
		var e = 90;
		d = void 0 === d ? 130 : d;
		e = void 0 === e ? 30 : e;
		var f = Od(jg, rd(a));
		if (!f) throw new p("No link unit size for width=" + a + "px");
		a = Math.min(a, 1200);
		f = f.height();
		b = Math.max(f, b);
		a = (new K(10, new P(a, Math.min(b, 15 == f ? e : d)))).a;
		b = a.minWidth();
		a = a.height();
		15 <= c && (a = c);
		return new K(10, new P(b, a))
	}
	function kg(a, b, c, d) {
		if (!Va(Wa(b), Td.f)) return a;
		if ("false" == d.google_full_width_responsive) return d.google_full_width_responsive_allowed = !1, d.gfwrnwer = 1, a;
		var e = Mb(a, b, c, d);
		if (!0 !== e) return d.google_full_width_responsive_allowed = !1, d.gfwrnwer = e, a;
		e = I(b);
		if (!e) return a;
		d.google_full_width_responsive_allowed = !0;
		Ia(b, c, c.parentElement, a, e, d);
		return e
	}
	function lg(a, b, c, d, e) {
		var f;
		(f = I(b)) ? 488 > I(b) ? b.innerHeight >= b.innerWidth ? (e.google_full_width_responsive_allowed = !0, Ia(b, c, c.parentElement, a, f, e), f = {
			j: f,
			l: !0
		}) : f = {
			j: a,
			l: 5
		} : f = {
			j: a,
			l: 4
		} : f = {
			j: a,
			l: 10
		};
		var g = f;
		f = g.j;
		g = g.l;
		if (!0 !== g || a == f) return new K(12, new C(a, d), null, !0, g, 100);
		a = Sd(f, "auto", b, c, e, !0);
		return new K(12, a.v, a.J, !0, g, a.u)
	}
	function $b(a) {
		var b = a.google_ad_format;
		if ("autorelaxed" == b) return $f(a) ? 9 : 5;
		if (Rd(b)) return 1;
		if ("link" == b) return 4;
		if ("fluid" == b) return 8
	}
	function Ud(a, b, c, d, e) {
		e = b.offsetWidth || (c.google_ad_resize || (void 0 === e ? !1 : e)) && J(b, d, "width", A) || c.google_ad_width || 0;
		var f = (f = mg(a, e, b, c, d)) ? f : Yb(e, c.google_ad_format, d, b, c);
		f.a.C(d, e, c, b);
		Xb(f, e, c);
		1 != a && (a = f.a.height(), b.style.height = a + "px")
	}
	function mg(a, b, c, d, e) {
		var f = d.google_ad_height || J(c, e, "height", A);
		switch (a) {
		case 5:
			return a = na(247, Ka, function() {
				return Pd(b, d.google_ad_format, e, c, d)
			}), f = a.j, a = a.l, !0 === a && b != f && Ia(e, c, c.parentElement, b, f, d), !0 === a ? d.google_full_width_responsive_allowed = !0 : (d.google_full_width_responsive_allowed = !1, d.gfwrnwer = a), ag(f, d);
		case 9:
			return bg(b, d);
		case 4:
			return a = kg(b, e, c, d), ig(a, ud(e, c), f, u(e, Vd.L) ? 250 : 190);
		case 8:
			return gg(b, e, c, f, d);
		case 10:
			return lg(b, e, c, f, d)
		}
	}
	function F(a) {
		this.h = [];
		this.b = a || window;
		this.a = 0;
		this.g = null;
		this.i = 0
	}
	function ac(a) {
		var b = Xa(189, ia(a.Ga, a));
		a.b.setTimeout(b, 0)
	}
	function Wd(a) {
		try {
			return a.sz()
		} catch (b) {
			return !1
		}
	}
	function Xd(a) {
		return !!a && ("object" === typeof a || "function" === typeof a) && Wd(a) && la(a.nq) && la(a.nqa) && la(a.al) && la(a.rl)
	}
	function bc() {
		if (La && Wd(La)) return La;
		var a = fd(),
			b = a.google_jobrunner;
		return Xd(b) ? La = b : a.google_jobrunner = La = new F(a)
	}
	function ng(a, b) {
		bc().nq(a, b)
	}
	function og(a, b) {
		bc().nqa(a, b)
	}
	function Yd(a, b) {
		this.b = a;
		this.a = b
	}
	function pg(a, b) {
		var c = X(b);
		if (c) {
			c = I(c);
			var d = N(a, b) || {},
				e = d.direction;
			if ("0px" === d.width && "none" != d.cssFloat) return -1;
			if ("ltr" === e && c) return Math.floor(Math.min(1200, c - a.getBoundingClientRect().left));
			if ("rtl" === e && c) return a = b.document.body.getBoundingClientRect().right - a.getBoundingClientRect().right, Math.floor(Math.min(1200, c - a - Math.floor((c - b.document.body.clientWidth) / 2)))
		}
		return -1
	}
	function Zd(a) {
		var b = this;
		this.a = a;
		a.google_iframe_oncopy || (a.google_iframe_oncopy = {
			handlers: {},
			upd: function(a, d) {
				var c = $d("rx", a),
					f = Number;
				a = a && (a = a.match("dt=([^&]+)")) && 2 == a.length ? a[1] : "";
				f = f(a);
				f = (new Date).getTime() - f;
				c = c.replace(/&dtd=(\d+|-?M)/, "&dtd=" + (1E5 <= f ? "M" : 0 <= f ? f : "-M"));
				b.set(d, c);
				return c
			}
		});
		this.b = a.google_iframe_oncopy
	}
	function $d(a, b) {
		var c = new RegExp("\\b" + a + "=(\\d+)"),
			d = c.exec(b);
		d && (b = b.replace(c, a + "=" + (+d[1] + 1 || 1)));
		return b
	}
	function qg() {
		var a = l;
		this.b = void 0 === a ? l : a;
		this.i = "https://securepubads.g.doubleclick.net/static/3p_cookie.html";
		this.a = 2;
		this.g = [];
		this.h = !1;
		a: {
			a = yc(!1, 50);
			var b = wc(l);
			b && a.unshift(b);
			a.unshift(l);
			var c;
			for (b = 0; b < a.length; ++b) try {
				var d = a[b],
					e = eb(d);
				if (e) {
					this.a = ae(e);
					if (2 != this.a) break a;
					!c && M(d) && (c = d)
				}
			} catch (f) {}
			this.b = c || this.b
		}
	}
	function fb(a) {
		if (2 != be(a)) {
			for (var b = 1 == be(a), c = 0; c < a.g.length; c++) try {
				a.g[c](b)
			} catch (d) {}
			a.g = []
		}
	}
	function ce(a) {
		var b = eb(a.b);
		b && 2 == a.a && (a.a = ae(b))
	}
	function be(a) {
		ce(a);
		return a.a
	}
	function rg(a) {
		var b = cc;
		b.g.push(a);
		if (2 != b.a) fb(b);
		else if (b.h || (Da(b.b, "message", function(a) {
			var c = eb(b.b);
			if (c && a.source == c && 2 == b.a) {
				switch (a.data) {
				case "3p_cookie_yes":
					b.a = 1;
					break;
				case "3p_cookie_no":
					b.a = 0
				}
				fb(b)
			}
		}), b.h = !0), eb(b.b)) fb(b);
		else {
			a = Ua(new Ba(b.b.document), "IFRAME");
			a.src = b.i;
			a.name = "detect_3p_cookie";
			a.style.visibility = "hidden";
			a.style.display = "none";
			a.onload = function() {
				ce(b);
				fb(b)
			};
			try {
				b.b.document.body.appendChild(a)
			} catch (c) {}
		}
	}

	function eb(a) {
		return a.frames && a.frames[za("detect_3p_cookie")] || null
	}
	function ae(a) {
		return Bc(a, "3p_cookie_yes") ? 1 : Bc(a, "3p_cookie_no") ? 0 : 2
	}
	function de(a) {
		return sg.test(a) && !tg.test(a)
	}
	function ug(a) {
		a = "https://" + ("adservice" + a + "/adsid/integrator.js");
		var b = ["domain=" + encodeURIComponent(l.location.hostname)];
		B[3] >= +new Date && b.push("adsid=" + encodeURIComponent(B[1]));
		return a + "?" + b.join("&")
	}
	function gb() {
		fa = l;
		B = fa.googleToken = fa.googleToken || {};
		var a = +new Date;
		B[1] && B[3] > a && 0 < B[2] || (B[1] = "", B[2] = -1, B[3] = -1, B[4] = "", B[6] = "");
		y = fa.googleIMState = fa.googleIMState || {};
		de(y[1]) || (y[1] = ".google.com");
		"array" == ba(y[5]) || (y[5] = []);
		ic(y[6]) || (y[6] = !1);
		"array" == ba(y[7]) || (y[7] = []);
		D(y[8]) || (y[8] = 0)
	}
	function vg(a) {
		gb();
		var b = fa.googleToken[5] || 0;
		a && (0 != b || B[3] >= +new Date ? L.T(a) : (L.W().push(a), L.ba()));
		B[3] >= +new Date && B[2] >= +new Date || L.ba()
	}
	function ee(a) {
		l.processGoogleToken = l.processGoogleToken ||
		function(a, c) {
			var b = a;
			b = void 0 === b ? {} : b;
			c = void 0 === c ? 0 : c;
			a = b.newToken || "";
			var e = "NT" == a,
				f = parseInt(b.freshLifetimeSecs || "", 10),
				g = parseInt(b.validLifetimeSecs || "", 10),
				h = b["1p_jar"] || "";
			b = b.pucrd || "";
			gb();
			1 == c ? L.Da() : L.Ca();
			var k = fa.googleToken = fa.googleToken || {},
				l = 0 == c && a && aa(a) && !e && D(f) && 0 < f && D(g) && 0 < g && aa(h);
			e = e && !L.H() && (!(B[3] >= +new Date) || "NT" == B[1]);
			var m = !(B[3] >= +new Date) && 0 != c;
			if (l || e || m) e = +new Date, f = e + 1E3 * f, g = e + 1E3 * g, 1E-5 > Math.random() && yb("https://pagead2.googlesyndication.com/pagead/gen_204?id=imerr&err=" + c, void 0), k[5] = c, k[1] = a, k[2] = f, k[3] = g, k[4] = h, k[6] = b, gb();
			if (l || !L.H()) {
				c = L.W();
				for (a = 0; a < c.length; a++) L.T(c[a]);
				c.length = 0
			}
		};
		vg(a)
	}
	function wg(a) {
		cc = cc || new qg;
		rg(function(b) {
			b && a()
		})
	}
	function xg() {
		var a = u(m, hb.f) || u(m, ib.f);
		a && m.google_sa_impl && !m.document.getElementById("google_shimpl") && (m.google_sa_queue = null, m.google_sl_win = null, m.google_sa_impl = null);
		a && !m.google_sa_queue && (m.google_sa_queue = [], m.google_sl_win = m, m.google_process_slots = function() {
			return fe(m)
		}, a = ge(), Vb(m.document, a, "preload", "script"), !E(H, "Chrome") && !E(H, "CriOS") || E(H, "Edge") ? tb(m.document, a).id = "google_shimpl" : (a = document.createElement("IFRAME"), a.id = "google_shimpl", a.style.display = "none", m.document.documentElement.appendChild(a), Gd(m, "google_shimpl", "<!doctype html><html><body>" + ("<" + R + ">") + "google_sl_win=window.parent;google_async_iframe_id='google_shimpl';" + ("</" + R + ">") + jb() + "</body></html>"), a.contentWindow.document.close()))
	}
	function jb(a) {
		return ["<", R, ' src="', ge(void 0 === a ? "/show_ads_impl.js" : a), '"></', R, ">"].join("")
	}
	function ge(a) {
		a = void 0 === a ? "/show_ads_impl.js" : a;
		var b = he ? "https" : "http";
		a: {
			if (da) try {
				var c = m.google_cafe_host || m.top.google_cafe_host;
				if (c) break a
			} catch (d) {}
			c = Ec(void 0)
		}
		return Za(c, ["/pagead/js/r20180924/r20180604", a, ""].join(""), b)
	}
	function ie(a, b, c, d) {
		return function() {
			var e = !1;
			d && bc().al(3E4);
			try {
				Gd(a, b, c), e = !0
			} catch (g) {
				var f = fd().google_jobrunner;
				Xd(f) && f.rl()
			}
			e && (e = $d("google_async_rrc", c), (new Zd(a)).set(b, ie(a, b, e, !1)))
		}
	}
	function yg(a) {
		var b = ["<iframe"];
		ca(a, function(a, d) {
			null != a && b.push(" " + d + '="' + lc(a) + '"')
		});
		b.push("></iframe>");
		return b.join("")
	}
	function je() {
		return Za(Dc(), ["/pagead/html/r20180924/r20180604/zrt_lookup.html#", encodeURIComponent("")].join(""))
	}
	function zg(a) {
		var b = document.createElement("IFRAME");
		b.id = "google_esf";
		b.name = "google_esf";
		b.src = je();
		ub(a, function(a, d) {
			b.setAttribute(d, a)
		});
		return b
	}
	function ke(a, b) {
		if (!kb) a: {
			for (var c = yc(), d = 0; d < c.length; d++) try {
				var e = c[d].frames.google_esf;
				if (e) {
					kb = e;
					break a
				}
			} catch (f) {}
			kb = null
		}
		if (!kb) {
			c = {};
			c = (c.style = "display:none", c);
			if (/[^a-z0-9-]/.test(a)) return "";
			c["data-ad-client"] = Ed(a);
			b ? a = zg(c) : (c.id = "google_esf", c.name = "google_esf", c.src = je(), a = yg(c));
			return a
		}
		return b ? null : ""
	}
	function Ag(a, b, c) {
		Bg(a, b, c, function(a, b, f) {
			a = a.document;
			for (var d = b.id, e = 0; !d || a.getElementById(d);) d = "aswift_" + e++;
			b.id = d;
			b.name = d;
			d = Number(f.google_ad_width);
			e = Number(f.google_ad_height);
			var k = f.ds || "";
			k && (k += k.endsWith(";") ? "" : ";");
			16 == f.google_reactive_ad_format ? (f = a.createElement("div"), a = Fd(b, d, e, k), f.innerHTML = a, c.appendChild(f.firstChild)) : (f = Fd(b, d, e, k), c.innerHTML = f);
			return b.id
		})
	}
	function Cg(a, b) {
		b = "{iframeWin: window, pubWin: window.parent, vars: " + ("window.parent['google_sv_map']['" + b + "']") + "}";
		if (u(a, hb.f) || u(a, ib.f)) return "<" + R + ">window.parent.google_sa_impl(" + b + ");</" + R + ">";
		b = "<" + R + ">window.google_process_slots=function(){" + ("window.google_sa_impl(" + b + ");") + ("};</" + R + ">");
		a = u(a, dc.f) ? jb("/show_ads_impl_le.js") : u(a, dc.c) ? jb("/show_ads_impl_le_c.js") : jb();
		return b + a
	}
	function Bg(a, b, c, d) {
		var e = {},
			f = b.google_ad_width,
			g = b.google_ad_height;
		null != f && (e.width = f && '"' + f + '"');
		null != g && (e.height = g && '"' + g + '"');
		e.frameborder = '"0"';
		e.marginwidth = '"0"';
		e.marginheight = '"0"';
		e.vspace = '"0"';
		e.hspace = '"0"';
		e.allowtransparency = '"true"';
		e.scrolling = '"no"';
		e.allowfullscreen = '"true"';
		e.onload = '"' + Dg + '"';
		d = d(a, e, b);
		f = b.google_ad_output;
		e = b.google_ad_format;
		g = b.google_ad_width || 0;
		var h = b.google_ad_height || 0;
		e || "html" != f && null != f || (e = g + "x" + h);
		f = !b.google_ad_slot || b.google_override_format || !le[b.google_ad_width + "x" + b.google_ad_height] && "aa" == b.google_loader_used;
		e && f ? e = e.toLowerCase() : e = "";
		b.google_ad_format = e;
		if (!D(b.google_reactive_sra_index) || !b.google_ad_unit_key) {
			e = [b.google_ad_slot, b.google_orig_ad_format || b.google_ad_format, b.google_ad_type, b.google_orig_ad_width || b.google_ad_width, b.google_orig_ad_height || b.google_ad_height];
			f = [];
			g = 0;
			for (h = c; h && 25 > g; h = h.parentNode, ++g) 9 === h.nodeType ? f.push("") : f.push(h.id);
			(f = f.join()) && e.push(f);
			b.google_ad_unit_key = vb(e.join(":")).toString();
			var k = void 0 === k ? !1 : k;
			e = [];
			for (f = 0; c && 25 > f; ++f) {
				g = "";
				void 0 !== k && k || (g = (g = 9 !== c.nodeType && c.id) ? "/" + g : "");
				a: {
					if (c && c.nodeName && c.parentElement) {
						h = c.nodeName.toString().toLowerCase();
						for (var l = c.parentElement.childNodes, m = 0, n = 0; n < l.length; ++n) {
							var q = l[n];
							if (q.nodeName && q.nodeName.toString().toLowerCase() === h) {
								if (c === q) {
									h = "." + m;
									break a
								}++m
							}
						}
					}
					h = ""
				}
				e.push((c.nodeName && c.nodeName.toString().toLowerCase()) + g + h);
				c = c.parentElement
			}
			k = e.join() + ":";
			c = a;
			e = [];
			if (c) try {
				var r = c.parent;
				for (f = 0; r && r !== c && 25 > f; ++f) {
					var p = r.frames;
					for (g = 0; g < p.length; ++g) if (c === p[g]) {
						e.push(g);
						break
					}
					c = r;
					r = c.parent
				}
			} catch (oh) {}
			b.google_ad_dom_fingerprint = vb(k + e.join()).toString()
		}
		p = "";
		u(a, me.f) ? (r = ke(b.google_ad_client, !0)) && a.document.documentElement.appendChild(r) : p = ke(b.google_ad_client, !1);
		r = u(a, hb.f) || u(a, ib.f);
		k = ne;
		c = (new Date).getTime();
		b.google_lrv = "r20180924";
		b.google_async_iframe_id = d;
		e = a;
		e = Ab(Ea(e)) || e;
		e = e.google_unique_id;
		b.google_unique_id = "number" === typeof e ? e : 0;
		b.google_start_time = k;
		b.google_bpp = c > k ? c - k : 1;
		b.google_async_rrc = 0;
		a.google_sv_map = a.google_sv_map || {};
		a.google_sv_map[d] = b;
		a.google_t12n_vars = Cd;
		p = ["<!doctype html><html><body>", p, "<" + R + ">", r ? "google_sl_win=window.parent;" : "", "google_iframe_start_time=new Date().getTime();", 'google_async_iframe_id="' + d + '";', "</" + R + ">", Cg(a, d), "</body></html>"].join("");
		b = a.document.getElementById(d) ? ng : og;
		d = ie(a, d, p, !0);
		r ? (a.google_sa_queue = a.google_sa_queue || [], a.google_sa_impl ? b(d) : a.google_sa_queue.push(d)) : b(d)
	}
	function Eg(a, b) {
		var c = navigator;
		a && b && c && (a = a.document, b = Ed(b), /[^a-z0-9-]/.test(b) || ((c = pb("r20160913")) && (c += "/"), tb(a, Za("pagead2.googlesyndication.com", "/pub-config/" + c + b + ".js"))))
	}
	function oe(a, b, c) {
		for (var d = a.attributes, e = d.length, f = 0; f < e; f++) {
			var g = d[f];
			if (/data-/.test(g.name)) {
				var h = pb(g.name.replace("data-matched-content", "google_content_recommendation").replace("data", "google").replace(/-/g, "_"));
				if (!b.hasOwnProperty(h)) {
					g = g.value;
					var k = {};
					k = (k.google_reactive_ad_format = Cc, k.google_allow_expandable_ads = Ca, k);
					g = k.hasOwnProperty(h) ? k[h](g, null) : g;
					null === g || (b[h] = g)
				}
			}
		}
		if (c.document && c.document.body && !$b(b) && !b.google_reactive_ad_format && (d = parseInt(a.style.width, 10), e = pg(a, c), 0 < e && d > e)) if (f = parseInt(a.style.height, 10), d = !! le[d + "x" + f], u(c, pe.D)) b.google_ad_resize = 0;
		else {
			h = e;
			if (d) if (g = df(e, f)) h = g, b.google_ad_format = g + "x" + f + "_0ads_al";
			else throw Error("TSS=" + e);
			b.google_ad_resize = 1;
			b.google_ad_width = h;
			d || (b.google_ad_format = null, b.google_override_format = !0);
			e = h;
			a.style.width = e + "px";
			f = Yb(e, "auto", c, a, b);
			h = e;
			f.a.C(c, h, b, a);
			Xb(f, h, b);
			f = f.a;
			b.google_responsive_formats = null;
			f.minWidth() > e && !d && (b.google_ad_width = f.minWidth(), a.style.width = f.minWidth() + "px")
		}
		d = b.google_reactive_ad_format;
		if (!b.google_enable_content_recommendations || 1 != d && 2 != d) {
			d = a.offsetWidth || J(a, c, "width", A) || b.google_ad_width || 0;
			a: if (e = Sa(Yb, d, "auto", c, a, b, !1, !0), f = u(c, ea.c), h = u(c, ea.f), g = Dd(c, b.google_ad_client), !(f || h || g) || !sb() || b.google_reactive_ad_format || $b(b) || md(a, b) || 1 == b.google_ad_resize || (da && c.google_top_window || c.top) != c) d = !1;
			else {
				for (h = a; h; h = h.parentElement) if (g = N(h, c), (k = !g) || (k = !(0 <= Ta(["static", "relative"], g.position))), k) {
					d = !1;
					break a
				}
				if (!0 !== Lb(c, a, d, .3, b)) d = !1;
				else {
					b.google_resizing_allowed = !0;
					h = u(c, Ma.c);
					g = u(c, Ma.f);
					k = Ja(c.location, "google_responsive_slot_debug") || Ja(c.location, "google_responsive_slot_preview") || cb(c, ea.c, ea.f, Ma.c, Ma.f);
					var l = n(Tb(), 142);
					if (k || Math.random() < l) b.ovlp = !0;
					f || g ? (f = {}, Xb(e(), d, f), b.google_resizing_width = f.google_ad_width, b.google_resizing_height = f.google_ad_height, f.ds && (b.ds = f.ds)) : b.google_ad_format = "auto";
					(d = h ? "AutoOptimizeAdSizeVariant" : g ? "AutoOptimizeAdSizeOriginal" : null) && (b.google_ad_channel = b.google_ad_channel ? [b.google_ad_channel, d].join("+") : d);
					d = !0
				}
			}
			if (e = $b(b)) Ud(e, a, b, c, d);
			else {
				if (md(a, b)) {
					if (d = N(a, c)) a.style.width = d.width, a.style.height = d.height, ld(d, b);
					b.google_ad_width || (b.google_ad_width = a.offsetWidth);
					b.google_ad_height || (b.google_ad_height = a.offsetHeight);
					b.google_loader_features_used = 256;
					d = Ea(c);
					b.google_responsive_auto_format = d ? d.data && "rspv" == d.data.autoFormat ? 13 : 14 : 12
				} else ld(a.style, b), b.google_ad_output && "html" != b.google_ad_output || 300 != b.google_ad_width || 250 != b.google_ad_height || (d = a.style.width, a.style.width = "100%", e = a.offsetWidth, a.style.width = d, b.google_available_width = e);
				c.location && "#gfwmrp" == c.location.hash || 12 == b.google_responsive_auto_format && "true" == b.google_full_width_responsive && !u(c, qe.f) ? Ud(10, a, b, c, !1) : u(c, re.f) && 12 == b.google_responsive_auto_format && (a = Mb(a.offsetWidth || parseInt(a.style.width, 10) || b.google_ad_width, c, a, b), !0 !== a ? (b.efwr = !1, b.gfwrnwer = a) : b.efwr = !0)
			}
		} else b.google_ad_width = I(c), b.google_ad_height = 50, a.style.display = "none"
	}
	function se(a) {
		return Fg.test(a.className) && "done" != a.getAttribute("data-adsbygoogle-status")
	}
	function te(a) {
		return se(a) && "reserved" != a.getAttribute("data-adsbygoogle-status")
	}
	function ue(a, b) {
		var c = window;
		a.setAttribute("data-adsbygoogle-status", "done");
		Gg(a, b, c)
	}
	function Gg(a, b, c) {
		var d = sa();
		d.google_spfd || (d.google_spfd = oe);
		(d = b.google_reactive_ads_config) || oe(a, b, c);
		if (!Hg(a, b, c)) {
			if (d) {
				if (ve) {
					if (d.page_level_pubvars && d.page_level_pubvars.pltais) return;
					throw new p("Only one 'enable_page_level_ads' allowed per page.");
				}
				ve = !0
			} else kf(c);
			we || (we = !0, Eg(c, b.google_ad_client));
			ca(Ig, function(a, d) {
				b[d] = b[d] || c[d]
			});
			b.google_loader_used = "aa";
			b.google_reactive_tag_first = 1 === Na;
			if ((d = b.google_ad_output) && "html" != d) throw new p("No support for google_ad_output=" + d);
			na(164, Ka, function() {
				Ag(c, b, a)
			})
		}
	}
	function Hg(a, b, c) {
		var d = b.google_reactive_ads_config;
		if (d) {
			var e = d.page_level_pubvars;
			e = (Ra(e) ? e : {}).google_tag_origin
		}
		if ("js" === b.google_ad_output) return !1;
		var f = e || b.google_tag_origin;
		e = aa(a.className) && /(\W|^)adsbygoogle-noablate(\W|$)/.test(a.className);
		var g = b.google_ad_slot,
			h = c.google_ad_modifications;
		!h || Hc(h.ad_whitelist, g, f) ? h = null : (f = h.space_collapsing || "none", h = (g = Hc(h.ad_blacklist, g)) ? {
			P: !0,
			fa: g.space_collapsing || f
		} : h.remove_ads_by_default ? {
			P: !0,
			fa: f,
			sa: h.dont_remove_atf
		} : null);
		if (e = h && h.P && "on" != b.google_adtest && !e) {
			a: {
				try {
					if (a.parentNode && 0 < a.offsetWidth && 0 < a.offsetHeight && a.style && "none" !== a.style.display && "hidden" !== a.style.visibility && (!a.style.opacity || 0 !== Number(a.style.opacity))) {
						var k = a.getBoundingClientRect(),
							m = 0 < k.right && 0 < k.bottom;
						break a
					}
				} catch (t) {}
				m = !1
			}
			m && (m = nd(a, c) < bb(c).clientHeight);
			e = !(m && h.sa)
		}
		if (e) return a.className += " adsbygoogle-ablated-ad-slot", c = c.google_sv_map = c.google_sv_map || {}, b.google_ad_slot && (c[b.google_ad_slot] = b, a.setAttribute("google_ad_slot", b.google_ad_slot)), "slot" == h.fa && (null !== Ac(a.getAttribute("width")) && a.setAttribute("width", 0), null !== Ac(a.getAttribute("height")) && a.setAttribute("height", 0), a.style.width = "0px", a.style.height = "0px"), !0;
		if ((m = N(a, c)) && "none" == m.display && !("on" == b.google_adtest || 0 < b.google_reactive_ad_format || d)) return c.document.createComment && a.appendChild(c.document.createComment("No ad requested because of display:none on the adsbygoogle tag")), !0;
		a = null == b.google_pgb_reactive || 3 === b.google_pgb_reactive;
		return 1 !== b.google_reactive_ad_format && 8 !== b.google_reactive_ad_format || !a ? !1 : (l.console && l.console.warn("Adsbygoogle tag with data-reactive-ad-format=" + b.google_reactive_ad_format + " is deprecated. Check out page-level ads at https://www.google.com/adsense"), !0)
	}
	function xe(a) {
		for (var b = document.getElementsByTagName("INS"), c = 0, d = b[c]; c < b.length; d = b[++c]) if (te(d) && (!a || d.id == a)) return d;
		return null
	}
	function Jg() {
		var a = document.createElement("INS");
		a.className = "adsbygoogle";
		a.className += " adsbygoogle-noablate";
		a.style.display = "none";
		return a
	}
	function Kg() {
		try {
			if (Va(Wa(m), ye.f)) {
				for (var a = document.getElementsByTagName("INS"), b = 0, c = 0; c < a.length; c++) {
					var d = a[c];
					if (!d) break;
					var e = N(d, m);
					!te(d) || e && "none" == e.display || b++
				}
				var f = window.adsbygoogle;
				f && (b -= f.length);
				return b
			}
		} catch (g) {
			return -9876
		}
	}
	function ze(a) {
		var b = {};
		ca(Lg, function(c, d) {
			!1 === a.enable_page_level_ads ? b[d] = !1 : a.hasOwnProperty(d) && (b[d] = a[d])
		});
		Ra(a.enable_page_level_ads) && (b.page_level_pubvars = a.enable_page_level_ads);
		var c = Jg();
		W.body.appendChild(c);
		var d = {};
		d = (d.google_reactive_ads_config = b, d.google_ad_client = a.google_ad_client, d);
		if (1 == a.enable_page_level_ads) {
			var e = Kg();
			void 0 !== e && (d.google_additional_ins_elements = e)
		}
		d.google_pause_ad_requests = Oa;
		ue(c, d)
	}
	function Mg(a) {
		var b = X(window);
		if (!b) throw new p("Page-level tag does not work inside iframes.");
		hd(b).wasPlaTagProcessed = !0;
		var c;
		ca(Wa(m), function(a) {
			switch (a) {
			case xa.ia:
				c = 0;
				break;
			case xa.ja:
				c = 100;
				break;
			case xa.ka:
				c = 200;
				break;
			case xa.la:
				c = 300
			}
		});
		b = c ?
		function() {
			return l.setTimeout(function() {
				return ze(a)
			}, c)
		} : function() {
			return ze(a)
		};
		W.body && void 0 === c || "complete" == W.readyState || "interactive" == W.readyState ? b() : Da(W, "DOMContentLoaded", bd(191, b))
	}
	function Ae(a) {
		var b = {};
		na(165, yf, function() {
			Ng(a, b)
		}, function(c) {
			c.client = c.client || b.google_ad_client || a.google_ad_client;
			c.slotname = c.slotname || b.google_ad_slot;
			c.tag_origin = c.tag_origin || b.google_tag_origin
		})
	}
	function Ng(a, b) {
		ne = (new Date).getTime();
		xg();
		a: {
			if (void 0 != a.enable_page_level_ads) {
				if (aa(a.google_ad_client)) {
					var c = !0;
					break a
				}
				throw new p("'google_ad_client' is missing from the tag config.");
			}
			c = !1
		}
		c ? Og(a, b) : (0 === Na && (Na = 2), (c = a.params) && ca(c, function(a, c) {
			b[c] = a
		}), "js" === b.google_ad_output ? console.warn("Ads with google_ad_output='js' have been deprecated and no longer work. Contact your AdSense account manager or switch to standard AdSense ads.") : (a = Pg(a.element), b.google_pause_ad_requests = Oa, ue(a, b)))
	}
	function Og(a, b) {
		0 === Na && (Na = 1);
		a.tag_partner && (Gc(l, a.tag_partner), Gc(b, a.tag_partner));
		b = a.google_ad_client;
		if (!Be) {
			Be = !0;
			try {
				var c = l.localStorage.getItem("google_ama_config")
			} catch (ec) {
				c = null
			}
			try {
				var d = c ? new Mc(c ? JSON.parse(c) : null) : null
			} catch (ec) {
				d = null
			}
			if (c = d) if (d = qa(c, Nc, 3), !d || v(d, 1) <= +new Date) try {
				l.localStorage.removeItem("google_ama_config")
			} catch (ec) {
				Ya(l, {
					lserr: 1
				})
			} else {
				if (qa(c, Cb, 13)) switch (d = !0, v(qa(c, Cb, 13), 1)) {
				case 1:
				case 2:
				case 3:
					d = !1;
				case 4:
					var e = l.google_ad_modifications = l.google_ad_modifications || {};
					e.remove_ads_by_default = !0;
					e.space_collapsing = "slot";
					e.dont_remove_atf = void 0 === d ? !1 : d
				}
				$a(3, [qb(c)]);
				d = Qg;
				try {
					var f = Aa(c, 5);
					if (0 < f.length) {
						var g = new Bb,
							h = f || [];
						2 < g.i ? g.b[2 + g.h] = h : (mc(g), g.g[2] = h);
						var k = g
					} else a: {
						h = l.location.pathname;
						var m = oc(c, Bb, 7);
						g = {};
						for (f = 0; f < m.length; ++f) {
							var n = v(m[f], 1);
							D(n) && !g[n] && (g[n] = m[f])
						}
						for (var p = h.replace(/(^\/)|(\/$)/g, "");;) {
							var q = vb(p);
							if (g[q]) {
								k = g[q];
								break a
							}
							if (!p) {
								k = null;
								break a
							}
							p = p.substring(0, p.lastIndexOf("/"))
						}
					}
					var r;
					if (r = k) a: {
						var u = Aa(k, 2);
						if (u) for (h = 0; h < u.length; h++) if (1 == u[h]) {
							r = !0;
							break a
						}
						r = !1
					}
					if (r) {
						var w = nc(c, 12);
						l.google_ama_all_ads_detection_enabled = w;
						if (v(k, 4)) {
							r = {};
							var y = new Ib(null, (r.google_package = v(k, 4), r));
							d = Af(d, y)
						}
						var x = new Pf;
						(new Rb(new Mf(b, c, d), x)).start();
						x.b.then(Sa(Rf, l), Sa(Sf, l))
					}
				} catch (ec) {
					Ya(l, {
						atf: -1
					})
				}
			}
		}
		Mg(a)
	}
	function Pg(a) {
		if (a) {
			if (!se(a) && (a.id ? a = xe(a.id) : a = null, !a)) throw new p("'element' has already been filled.");
			if (!("innerHTML" in a)) throw new p("'element' is not a good DOM element.");
		} else if (a = xe(), !a) throw new p("All ins elements in the DOM with class=adsbygoogle already have ads in them.");
		return a
	}
	function Ce() {
		vf();
		na(166, zf, Rg)
	}
	function Rg() {
		var a = Ab(Ea(m)) || m,
			b = a.google_ad_modifications = a.google_ad_modifications || {};
		if (!b.plle) {
			b.plle = !0;
			var c = b.eids = b.eids || [];
			b = b.loeids = b.loeids || [];
			var d = Tb(),
				e = Tb(),
				f = X(a) || a;
			f = Ja(f.location, "google_responsive_slot_debug") || Ja(f.location, "google_responsive_slot_preview");
			var g = Dd(a);
			f ? (f = ea, g = De, e = f.f) : g ? (f = Ma, g = Sg, e = oa(a, new ma(0, 999), n(e, 120), n(e, 121), [f.c, f.f], 2)) : (f = ea, g = De, e = oa(a, Tg, n(e, 96), n(e, 97), [f.c, f.f]));
			if (e) {
				var h = {};
				f = (h[f.c] = g.c, h[f.f] = g.f, h)[e];
				e = {
					ya: e,
					Aa: f
				}
			} else e = null;
			f = e || {};
			e = f.ya;
			f = f.Aa;
			e && f && (q(c, e), q(c, f));
			g = qe;
			e = x(a, n(d, 136), [g.c, g.f]);
			q(c, e);
			g = ye;
			e = oa(a, Ug, n(d, 144), n(d, 145), [g.c, g.f]);
			q(b, e);
			h = Vg;
			e == g.c ? f = h.c : e == g.f ? f = h.f : f = "";
			q(c, f);
			g = Vd;
			q(c, oa(a, Wg, n(d, 9), n(d, 10), [g.c, g.L]));
			g = Td;
			e = oa(a, Xg, n(d, 108), n(d, 109), [g.c, g.f]);
			q(b, e);
			h = Yg;
			e == g.c ? f = h.c : e == g.f ? f = h.f : f = "";
			q(c, f);
			pb("") && q(b, "");
			g = pe;
			e = x(a, n(d, 11), [g.c, g.D]);
			q(c, e);
			h = "";
			e === g.c ? h = "62710018" : e === g.D && (h = "62710017");
			q(c, h);
			g = ib;
			e = oa(a, Zg, n(d, 115), n(d, 116), [g.c, g.f]);
			q(c, e);
			e || (g = hb, e = x(a, n(d, 12), [g.c, g.f]), q(c, e));
			g = $g;
			e = x(a, n(d, 146), [g.c, g.f]);
			q(c, e);
			g = dc;
			e = x(a, n(d, 56), [g.c, g.f]);
			q(c, e);
			g = fc;
			e = x(a, n(d, 13), [g.m, g.c]);
			q(c, e);
			e = x(a, 0, [g.K]);
			q(c, e);
			g = Ee;
			e = x(a, n(d, 60), [g.m, g.c]);
			q(c, e);
			e == Ee.m && (g = gc, e = x(a, n(d, 66), [g.m, g.c]), q(c, e), g = ah, e = x(a, n(d, 137), [g.m, g.c]), q(c, e), e == gc.m && (g = bh, e = x(a, n(d, 135), [g.m, g.c]), q(c, e)));
			g = re;
			e = x(a, n(d, 98), [g.c, g.f]);
			q(c, e);
			if (Ca(d.a[77], !1) || da) g = ya, e = x(a, n(d, 76), [g.c, g.O, g.F, g.N]), q(c, e), e || (e = x(a, n(d, 83), [g.M]), q(c, e));
			g = Fe;
			e = x(a, n(d, 92), [g.c, g.f]);
			q(c, e);
			g = Qd;
			e = oa(a, ch, n(d, 99), n(d, 100), [g.c, g.f]);
			q(b, e);
			h = dh;
			e == g.c ? f = h.c : e == g.f ? f = h.f : f = "";
			q(c, f);
			e = [];
			f = 0;
			for (var k in xa) e[f++] = xa[k];
			q(b, x(a, n(d, 114), e));
			g = eh;
			e = x(a, n(d, 127), [g.c, g.ma, g.na]);
			q(c, e);
			g = me;
			e = x(a, n(d, 141), [g.f, g.c]);
			q(c, e)
		}
		Hb(u(m, gc.m) || u(m, fc.m) || u(m, fc.K));
		if (u(m, ya.O) || u(m, ya.F) || u(m, ya.N) || u(m, ya.M)) gb(), de(".google.cn") && (y[1] = ".google.cn"), u(m, ya.F) ? (a = ff(), wg(a), ee(a)) : ee(null);
		u(m, Fe.f) && (a = Df() ? zb("", "pagead2.googlesyndication.com") : Dc(), Vb(sa().document, a, "preconnect"));
		if (a = X(l)) a = hd(a), Ra(a.tagSpecificState) && a.tagSpecificState[1] || !Ra(a.tagSpecificState) || (a.tagSpecificState[1] = new Tf);
		a = window.adsbygoogle;
		if (!a || !a.loaded) {
			c = {
				push: Ae,
				loaded: !0
			};
			try {
				Object.defineProperty(c, "requestNonPersonalizedAds", {
					set: fh
				}), Object.defineProperty(c, "pauseAdRequests", {
					set: gh
				}), Object.defineProperty(c, "onload", {
					set: hh
				})
			} catch (t) {}
			a && (void 0 !== a.requestNonPersonalizedAds && (c.requestNonPersonalizedAds = a.requestNonPersonalizedAds), void 0 !== a.pauseAdRequests && (c.pauseAdRequests = a.pauseAdRequests));
			if (a && a.shift) try {
				var p;
				for (k = 20; 0 < a.length && (p = a.shift()) && 0 < k;) Ae(p), --k
			} catch (t) {
				throw window.setTimeout(Ce, 0), t;
			}
			window.adsbygoogle = c;
			a && (c.onload = a.onload)
		}
	}
	function fh(a) {
		if (+a) {
			if ((a = xc()) && a.frames && !a.frames.GoogleSetNPA) try {
				var b = a.document,
					c = new Ba(b),
					d = b.body || b.head && b.head.parentElement;
				if (d) {
					var e = Ua(c, "IFRAME");
					e.name = "GoogleSetNPA";
					e.id = "GoogleSetNPA";
					e.setAttribute("style", "display:none;position:fixed;left:-999px;top:-999px;width:0px;height:0px;");
					d.appendChild(e)
				}
			} catch (f) {}
		} else(b = xc().document.getElementById("GoogleSetNPA")) && b.parentNode && b.parentNode.removeChild(b)
	}
	function gh(a) {
		+a ? Oa = !0 : (Oa = !1, a = function() {
			if (!Oa) {
				var a = sa(),
					c = sa();
				try {
					if (W.createEvent) {
						var d = W.createEvent("CustomEvent");
						d.initCustomEvent("adsbygoogle-pub-unpause-ad-requests-event", !1, !1, "");
						a.dispatchEvent(d)
					} else if (la(c.CustomEvent)) {
						var e = new c.CustomEvent("adsbygoogle-pub-unpause-ad-requests-event", {
							bubbles: !1,
							cancelable: !1,
							detail: ""
						});
						a.dispatchEvent(e)
					} else if (la(c.Event)) {
						var f = new Event("adsbygoogle-pub-unpause-ad-requests-event", {
							bubbles: !1,
							cancelable: !1
						});
						a.dispatchEvent(f)
					}
				} catch (g) {}
			}
		}, l.setTimeout(a, 0), l.setTimeout(a, 1E3))
	}
	function hh(a) {
		la(a) && window.setTimeout(a, 0)
	}
	var Me = "function" == typeof Object.create ? Object.create : function(a) {
			function b() {}
			b.prototype = a;
			return new b
		};
	if ("function" == typeof Object.setPrototypeOf) var Ge = Object.setPrototypeOf;
	else {
		a: {
			var ih = {
				pa: !0
			},
				He = {};
			try {
				He.__proto__ = ih;
				var Ie = He.pa;
				break a
			} catch (a) {}
			Ie = !1
		}
		Ge = Ie ?
		function(a, b) {
			a.__proto__ = b;
			if (a.__proto__ !== b) throw new TypeError(a + " is not extensible");
			return a
		} : null
	}
	var hc = Ge,
		Oe = "function" == typeof Object.defineProperties ? Object.defineProperty : function(a, b, c) {
			a != Array.prototype && a != Object.prototype && (a[b] = c.value)
		},
		Ne = "undefined" != typeof window && window === this ? this : "undefined" != typeof global && null != global ? global : this;
	Qa("String.prototype.endsWith", function(a) {
		return a ? a : function(a, c) {
			if (null == this) throw new TypeError("The 'this' value for String.prototype.endsWith must not be null or undefined");
			if (a instanceof RegExp) throw new TypeError("First argument to String.prototype.endsWith must not be a regular expression");
			void 0 === c && (c = this.length);
			c = Math.max(0, Math.min(c | 0, this.length));
			for (var b = a.length; 0 < b && 0 < c;) if (this[--c] != a[--b]) return !1;
			return 0 >= b
		}
	});
	var jh = "function" == typeof Object.assign ? Object.assign : function(a, b) {
			for (var c = 1; c < arguments.length; c++) {
				var d = arguments[c];
				if (d) for (var e in d) Object.prototype.hasOwnProperty.call(d, e) && (a[e] = d[e])
			}
			return a
		};
	Qa("Object.assign", function(a) {
		return a || jh
	});
	Qa("Object.is", function(a) {
		return a ? a : function(a, c) {
			return a === c ? 0 !== a || 1 / a === 1 / c : a !== a && c !== c
		}
	});
	Qa("Number.isNaN", function(a) {
		return a ? a : function(a) {
			return "number" === typeof a && isNaN(a)
		}
	});
	var l = this,
		Pe = /^[\w+/_-]+[=]{0,2}$/,
		nb = null,
		ne = (new Date).getTime(),
		Ve = /&/g,
		We = /</g,
		Xe = />/g,
		Ye = /"/g,
		Ze = /'/g,
		$e = /\x00/g,
		Ue = /[\x00&<>"']/,
		Wb = {
			"\x00": "\\0",
			"\b": "\\b",
			"\f": "\\f",
			"\n": "\\n",
			"\r": "\\r",
			"\t": "\\t",
			"\x0B": "\x0B",
			'"': '\\"',
			"\\": "\\\\",
			"<": "<"
		},
		db = {
			"'": "\\'"
		};
	a: {
		var Je = l.navigator;
		if (Je) {
			var Ke = Je.userAgent;
			if (Ke) {
				var H = Ke;
				break a
			}
		}
		H = ""
	}
	za[" "] = ob;
	var cf = "function" == typeof Uint8Array,
		ja = [];
	Q.prototype.toString = function() {
		pc(this);
		return this.b.toString()
	};
	var W = document,
		m = window,
		le = {
			"120x90": !0,
			"160x90": !0,
			"180x90": !0,
			"200x90": !0,
			"468x15": !0,
			"728x15": !0
		};
	ra.prototype.g = !0;
	ra.prototype.a = function() {
		return this.b
	};
	var rc = {};
	ka.prototype.g = !0;
	ka.prototype.a = function() {
		return this.I
	};
	var Wf = /^(?:(?:https?|mailto|ftp):|[^:/?#]*(?:[/?#]|$))/i,
		sc = {};
	tc("about:blank");
	Ba.prototype.contains = function(a, b) {
		if (!a || !b) return !1;
		if (a.contains && 1 == b.nodeType) return a == b || a.contains(b);
		if ("undefined" != typeof a.compareDocumentPosition) return a == b || !! (a.compareDocumentPosition(b) & 16);
		for (; b && a != b;) b = b.parentNode;
		return b == a
	};
	var gf = ob,
		Uc = qc(function() {
			return E(H, "Google Web Preview") || 1E-4 > Math.random()
		}),
		xb = /^([0-9.]+)px$/,
		wb = /^(-?[0-9.]{1,30})$/,
		Xf = Object.assign ||
	function(a, b) {
		for (var c = 1; c < arguments.length; c++) {
			var d = arguments[c];
			if (d) for (var e in d) Object.prototype.hasOwnProperty.call(d, e) && (a[e] = d[e])
		}
		return a
	}, jf = /^([\w-]+\.)*([\w-]{2,})(:[0-9]+)?$/, Jd = Cc("468", 0), da = Ca("false", !1), kh = Ca("true", !1), he = Ca("false", !1), Bf = he || !kh, Fc = !! window.google_async_iframe_id, ta = Fc && window.parent || window, Fg = /(^| )adsbygoogle($| )/, Lg = {
		overlays: 1,
		interstitials: 2,
		vignettes: 2,
		inserts: 3,
		immersives: 4,
		list_view: 5,
		full_page: 6
	}, lh = {
		Ja: {
			name: "adFormat",
			o: D
		},
		Ia: {
			name: "adClient",
			o: /^[a-z0-9-]+$/i
		},
		La: {
			name: "adTest",
			o: /^on$/i
		},
		Oa: {
			name: "reqSrc",
			o: D
		},
		Na: {
			name: "pubVars",
			o: null
		},
		Ka: {
			name: "adKey",
			o: D
		},
		Ma: {
			name: "enabledInAsfe",
			o: ic
		}
	}, Le = [{
		name: "google_ad_channel",
		o: {
			predicate: /^[a-z0-9_-]+$/i,
			nullOnInvalid: !0
		}
	}, {
		name: "google_reactive_sra_index",
		o: {
			predicate: D,
			nullOnInvalid: !0
		}
	}, {
		name: "google_ad_unit_key",
		o: {
			predicate: D,
			nullOnInvalid: !0
		}
	}];
	U(Ic, Q);
	U(Jc, Q);
	U(Kc, Q);
	U(Lc, Q);
	var lf = [4],
		mf = [6, 7, 9, 10, 11];
	U(Mc, Q);
	U(Nc, Q);
	U(Bb, Q);
	U(Cb, Q);
	var nf = [1, 2, 5, 7],
		of = [2],
		mh = /^https?:\/\/(\w|-)+\.cdn\.ampproject\.(net|org)(\?|\/|$)/,
		mb = null,
		O = l.performance,
		nh = !! (O && O.mark && O.measure && O.clearMarks),
		Ga = qc(function() {
			var a;
			if (a = nh) {
				var b;
				if (null === mb) {
					mb = "";
					try {
						a = "";
						try {
							a = l.top.location.hash
						} catch (c) {
							a = l.location.hash
						}
						a && (mb = (b = a.match(/\bdeid=([\d,]+)/)) ? b[1] : "")
					} catch (c) {}
				}
				b = mb;
				a = !! b.indexOf && 0 <= b.indexOf("1337")
			}
			return a
		});
	Xc.prototype.start = function(a, b) {
		if (!this.a) return null;
		var c = Wc() || Vc();
		a = new uf(a, b, c);
		b = "goog_" + a.uniqueId + "_start";
		O && Ga() && O.mark(b);
		return a
	};
	Zc.prototype.a = function(a, b, c, d, e) {
		e = e || "jserror";
		try {
			var f = new Db;
			f.h = !0;
			ua(f, 1, "context", a);
			b.error && b.meta && b.id || (b = new Gb(Ha(b), b.fileName, b.lineNumber));
			b.msg && ua(f, 2, "msg", b.msg.substring(0, 512));
			b.file && ua(f, 3, "file", b.file);
			0 < b.line && ua(f, 4, "line", b.line);
			var g = b.meta || {};
			if (this.g) try {
				this.g(g)
			} catch (lb) {}
			if (d) try {
				d(g)
			} catch (lb) {}
			b = [g];
			f.a.push(5);
			f.b[5] = b;
			d = l;
			b = [];
			g = null;
			do {
				var h = d;
				if (M(h)) {
					var k = h.location.href;
					g = h.document && h.document.referrer || null
				} else k = g, g = null;
				b.push(new Pc(k || "", h));
				try {
					d = h.parent
				} catch (lb) {
					d = null
				}
			} while (d && h != d);
			k = 0;
			for (var m = b.length - 1; k <= m; ++k) b[k].depth = m - k;
			h = l;
			if (h.location && h.location.ancestorOrigins && h.location.ancestorOrigins.length == b.length - 1) for (m = 1; m < b.length; ++m) {
				var n = b[m];
				n.url || (n.url = h.location.ancestorOrigins[m - 1] || "", n.X = !0)
			}
			var p = new Pc(l.location.href, l, !1);
			h = null;
			var q = b.length - 1;
			for (n = q; 0 <= n; --n) {
				var r = b[n];
				!h && mh.test(r.url) && (h = r);
				if (r.url && !r.X) {
					p = r;
					break
				}
			}
			r = null;
			var u = b.length && b[q].url;
			0 != p.depth && u && (r = b[q]);
			var v = new pf(p, r);
			v.b && ua(f, 6, "top", v.b.url || "");
			ua(f, 7, "url", v.a.url || "");
			Eb(this.w, e, f, this.i, c)
		} catch (lb) {
			try {
				Eb(this.w, e, {
					context: "ecmserr",
					rctx: a,
					msg: Ha(lb),
					url: v && v.a.url
				}, this.i, c)
			} catch (ph) {}
		}
		return this.h
	};
	ha(Gb, Oc);
	ha(p, Error);
	var Fa = sa(),
		Fb = new Xc,
		$c = new function() {
			var a = void 0 === a ? m : a;
			this.h = "http:" === a.location.protocol ? "http:" : "https:";
			this.b = "pagead2.googlesyndication.com";
			this.g = "/pagead/gen_204?id=";
			this.a = .01;
			this.i = Math.random()
		},
		Y = new Zc;
	"complete" == Fa.document.readyState ? Hb() : Fb.a && Da(Fa, "load", function() {
		Hb()
	});
	var Ka = Y.a,
		Qg = new Ib(["google-auto-placed"], {
			google_tag_origin: "qs"
		}),
		va = null;
	U(ed, Q);
	var Jb = null,
		Vg = {
			c: "368226900",
			f: "368226901"
		},
		ye = {
			c: "368226910",
			f: "368226911"
		},
		qe = {
			c: "368226500",
			f: "368226501"
		},
		re = {
			c: "36998750",
			f: "36998751"
		},
		Vd = {
			c: "4089040",
			L: "4089042"
		},
		Yg = {
			c: "40993900",
			f: "40993901"
		},
		Td = {
			c: "40993910",
			f: "40993911"
		},
		fc = {
			m: "20040067",
			c: "20040068",
			K: "1337"
		},
		Ee = {
			c: "21060548",
			m: "21060549"
		},
		gc = {
			c: "21060623",
			m: "21060624"
		},
		bh = {
			c: "22324606",
			f: "22324607"
		},
		ah = {
			c: "21062271",
			m: "21062272"
		},
		pe = {
			D: "62710015",
			c: "62710016"
		},
		hb = {
			c: "21070024",
			f: "21070025"
		},
		ib = {
			c: "21070013",
			f: "21070014"
		},
		dc = {
			c: "21060518",
			f: "21060519"
		},
		ya = {
			c: "21060849",
			O: "21060850",
			F: "21060851",
			N: "21060852",
			M: "21060853"
		},
		Fe = {
			c: "21061394",
			f: "21061395"
		},
		ea = {
			c: "182982000",
			f: "182982100"
		},
		De = {
			c: "182982200",
			f: "182982300"
		},
		Ma = {
			c: "182983000",
			f: "182983100"
		},
		Sg = {
			c: "182983200",
			f: "182983300"
		},
		Qd = {
			c: "10583695",
			f: "10583696"
		},
		dh = {
			c: "10593695",
			f: "10593696"
		},
		xa = {
			c: "21062077",
			ia: "21062078",
			ja: "21062079",
			ka: "21062080",
			la: "21062081"
		},
		eh = {
			c: "10573695",
			ma: "10573696",
			na: "10573697"
		},
		me = {
			c: "20195144",
			f: "20195143"
		},
		$g = {
			c: "21062569",
			f: "21062570"
		},
		gd = {
			google: 1,
			googlegroups: 1,
			gmail: 1,
			googlemail: 1,
			googleimages: 1,
			googleprint: 1
		},
		Ef = /(corp|borg)\.google\.com:\d+$/;
	ab.prototype.getVerifier = function(a) {
		return this.b ? this.b[a] : null
	};
	ab.prototype.getSchema = function(a) {
		return this.a ? this.a(a) : null
	};
	ab.prototype.doesReturnAnotherSchema = function() {
		return this.a ? !0 : !1
	};
	Kb.prototype.toString = function() {
		return JSON.stringify({
			nativeQuery: this.h,
			occurrenceIndex: this.b,
			paragraphIndex: this.g,
			ignoreMode: this.a
		})
	};
	var qd = {
		rectangle: 1,
		horizontal: 2,
		vertical: 4
	},
		G = C.prototype;
	G.minWidth = function() {
		return this.b
	};
	G.height = function() {
		return this.a
	};
	G.s = function(a) {
		return 300 < a && 300 < this.a ? this.b : Math.min(1200, Math.round(a))
	};
	G.G = function(a) {
		return this.s(a) + "x" + this.height()
	};
	G.C = function() {};
	var Lf = {
		1: 1,
		2: 2,
		3: 3,
		0: 0
	},
		Of = {
			1: 0,
			2: 1,
			3: 2,
			4: 3
		};
	Ad.prototype.then = function(a, b) {
		if (this.b) throw Error("Then functions already set.");
		this.b = a;
		this.g = b;
		Qb(this)
	};
	Rb.prototype.start = function() {
		this.h()
	};
	Rb.prototype.h = function() {
		try {
			switch (this.b.document.readyState) {
			case "complete":
			case "interactive":
				zd(this.g, !0);
				Sb(this);
				break;
			default:
				zd(this.g, !1) ? Sb(this) : this.b.setTimeout(ia(this.h, this), 100)
			}
		} catch (a) {
			Sb(this, a)
		}
	};
	Y.h = !da;
	var Cd = {
		9: "400",
		10: "100",
		11: "0.10",
		12: "0.1",
		13: "0.001",
		19: "0.01",
		22: "0.01",
		23: "0.2",
		24: "0.05",
		27: "0.001",
		28: "0.001",
		29: "0.01",
		34: "0.001",
		37: "0.0",
		40: "0.15",
		43: "0.1",
		47: "0.01",
		56: "0.001",
		60: "0.03",
		66: "0.0",
		67: "0.04",
		76: "0.004",
		77: "true",
		78: "0.1",
		79: "1200",
		82: "3",
		83: "1.0",
		92: "0.02",
		96: "700",
		97: "10",
		98: "0.01",
		99: "600",
		100: "100",
		103: "0.01",
		108: "500",
		109: "100",
		111: "0.1",
		112: "0",
		114: "0.01",
		115: "170",
		116: "30",
		117: "0.02",
		118: "false",
		120: "0",
		121: "1000",
		126: "0.001",
		127: "0.1",
		128: "false",
		129: "0.02",
		135: "0.0",
		136: "0.02",
		137: "0.01",
		138: "0.01",
		141: "0.1",
		142: "0.01",
		143: "0.06",
		144: "0",
		145: "2",
		146: "0.02"
	},
		Ub = null;
	(function(a) {
		function b(a) {
			a.o && (c[a.name] = a.o)
		}
		var c = {
			msg_type: /^[a-zA-Z0-9_-]+$/
		};
		ca(lh, b);
		for (var d = 0; d < Le.length; d++) b(Le[d]);
		for (d = 0; d < a.length; d++) b(a[d]);
		return new ab(c)
	})([]);
	var Ug = new ma(0, 169),
		Zg = new ma(170, 199),
		Wg = new ma(400, 499),
		Xg = new ma(500, 599),
		ch = new ma(600, 699),
		Tg = new ma(700, 799),
		kb = null;
	ha(w, C);
	w.prototype.J = function() {
		return this.B
	};
	w.prototype.C = function(a, b, c, d) {
		1 != c.google_ad_resize && (d.style.height = this.height() + "px")
	};
	var Hd = ["google_content_recommendation_ui_type", "google_content_recommendation_columns_num", "google_content_recommendation_rows_num"],
		S = {},
		Kd = (S.image_stacked = 1 / 1.91, S.image_sidebyside = 1 / 3.82, S.mobile_banner_image_sidebyside = 1 / 3.82, S.pub_control_image_stacked = 1 / 1.91, S.pub_control_image_sidebyside = 1 / 3.82, S.pub_control_image_card_stacked = 1 / 1.91, S.pub_control_image_card_sidebyside = 1 / 3.74, S.pub_control_text = 0, S.pub_control_text_card = 0, S),
		T = {},
		Ld = (T.image_stacked = 80, T.image_sidebyside = 0, T.mobile_banner_image_sidebyside = 0, T.pub_control_image_stacked = 80, T.pub_control_image_sidebyside = 0, T.pub_control_image_card_stacked = 85, T.pub_control_image_card_sidebyside = 0, T.pub_control_text = 80, T.pub_control_text_card = 80, T),
		pa = {},
		cg = (pa.pub_control_image_stacked = 100, pa.pub_control_image_sidebyside = 200, pa.pub_control_image_card_stacked = 150, pa.pub_control_image_card_sidebyside = 250, pa.pub_control_text = 100, pa.pub_control_text_card = 150, pa);
	ha(Z, C);
	Z.prototype.s = function(a) {
		return Math.min(1200, Math.max(this.minWidth(), Math.round(a)))
	};
	ha(wa, C);
	wa.prototype.s = function() {
		return this.minWidth()
	};
	wa.prototype.C = function(a, b, c, d) {
		var e = this.s(b);
		Ia(a, d, d.parentElement, b, e, c);
		1 != c.google_ad_resize && (d.style.height = this.height() + "px")
	};
	var z = [new w(970, 90, 2), new w(728, 90, 2), new w(468, 60, 2), new w(336, 280, 1), new w(320, 100, 2), new w(320, 50, 2), new w(300, 600, 4), new w(300, 250, 1), new w(250, 250, 1), new w(234, 60, 2), new w(200, 200, 1), new w(180, 150, 1), new w(160, 600, 4), new w(125, 125, 1), new w(120, 600, 4), new w(120, 240, 4), new w(120, 120, 1, !0)],
		eg = [z[6], z[12], z[3], z[0], z[7], z[14], z[1], z[8], z[10], z[4], z[15], z[2], z[11], z[5], z[13], z[9], z[16]],
		hg = {
			"image-top": function(a) {
				return 600 >= a ? 284 + .414 * (a - 250) : 429
			},
			"image-middle": function(a) {
				return 500 >= a ? 196 - .13 * (a - 250) : 164 + .2 * (a - 500)
			},
			"image-side": function(a) {
				return 500 >= a ? 205 - .28 * (a - 250) : 134 + .21 * (a - 500)
			},
			"text-only": function(a) {
				return 500 >= a ? 187 - .228 * (a - 250) : 130
			},
			"in-article": function(a) {
				return 420 >= a ? a / 1.2 : 460 >= a ? a / 1.91 + 130 : 800 >= a ? a / 4 : 200
			}
		};
	ha(Zb, C);
	Zb.prototype.s = function() {
		return Math.min(1200, this.minWidth())
	};
	ha(P, C);
	P.prototype.s = function() {
		return this.minWidth()
	};
	P.prototype.G = function(a) {
		return C.prototype.G.call(this, a) + "_0ads_al"
	};
	var jg = [new P(728, 15), new P(468, 15), new P(200, 90), new P(180, 90), new P(160, 90), new P(120, 90)],
		La;
	G = F.prototype;
	G.ta = function(a, b) {
		0 != this.a || 0 != this.h.length || b && b != window ? this.V(a, b) : (this.a = 2, this.da(new Yd(a, window)))
	};
	G.V = function(a, b) {
		this.h.push(new Yd(a, b || this.b));
		ac(this)
	};
	G.za = function(a) {
		this.a = 1;
		if (a) {
			var b = Xa(188, ia(this.ca, this, !0));
			this.g = this.b.setTimeout(b, a)
		}
	};
	G.ca = function(a) {
		a && ++this.i;
		1 == this.a && (null != this.g && (this.b.clearTimeout(this.g), this.g = null), this.a = 0);
		ac(this)
	};
	G.Ea = function() {
		return !(!window || !Array)
	};
	G.ua = function() {
		return this.i
	};
	G.Ga = function() {
		if (0 == this.a && this.h.length) {
			var a = this.h.shift();
			this.a = 2;
			var b = Xa(190, ia(this.da, this, a));
			a.a.setTimeout(b, 0);
			ac(this)
		}
	};
	G.da = function(a) {
		this.a = 0;
		a.b()
	};
	F.prototype.nq = F.prototype.ta;
	F.prototype.nqa = F.prototype.V;
	F.prototype.al = F.prototype.za;
	F.prototype.rl = F.prototype.ca;
	F.prototype.sz = F.prototype.Ea;
	F.prototype.tc = F.prototype.ua;
	Zd.prototype.set = function(a, b) {
		var c = this;
		this.b.handlers[a] = b;
		this.a.addEventListener && this.a.addEventListener("load", function() {
			var b = c.a.document.getElementById(a);
			try {
				var e = b.contentWindow.document;
				if (b.onload && e && (!e.body || !e.body.firstChild)) b.onload()
			} catch (f) {}
		}, !1)
	};
	var Dg = lc("var i=this.id,s=window.google_iframe_oncopy,H=s&&s.handlers,h=H&&H[i],w=this.contentWindow,d;try{d=w.document}catch(e){}if(h&&d&&(!d.body||!d.body.firstChild)){if(h.call){setTimeout(h,0)}else if(h.match){try{h=s.upd(h,i)}catch(e){}w.location.replace(h)}}"),
		Pa = {},
		Ig = (Pa.google_ad_modifications = !0, Pa.google_analytics_domain_name = !0, Pa.google_analytics_uacct = !0, Pa.google_pause_ad_requests = !0, Pa),
		sg = /^\.google\.(com?\.)?[a-z]{2,3}$/,
		tg = /\.(cn|com\.bi|do|sl|ba|by|ma|am)$/,
		fa = l,
		cc, B, y, L = {
			H: function() {
				return 0 < y[8]
			},
			Ba: function() {
				y[8]++
			},
			Ca: function() {
				0 < y[8] && y[8]--
			},
			Da: function() {
				y[8] = 0
			},
			Ra: function() {
				return !1
			},
			W: function() {
				return y[5]
			},
			T: function(a) {
				try {
					a()
				} catch (b) {
					l.setTimeout(function() {
						throw b;
					}, 0)
				}
			},
			ba: function() {
				if (!L.H()) {
					var a = l.document,
						b = function(b) {
							b = ug(b);
							a: {
								try {
									var c = jc();
									break a
								} catch (h) {}
								c = void 0
							}
							var d = c;
							Vb(a, b, "preload", "script", d);
							c = a.createElement("script");
							c.type = "text/javascript";
							d && (c.nonce = d);
							c.onerror = function() {
								return l.processGoogleToken({}, 2)
							};
							b = vc(b);
							uc(c, b);
							try {
								(a.head || a.body || a.documentElement).appendChild(c), L.Ba()
							} catch (h) {}
						},
						c = y[1];
					b(c);
					".google.com" != c && b(".google.com");
					b = {};
					var d = (b.newToken = "FBT", b);
					l.setTimeout(function() {
						return l.processGoogleToken(d, 1)
					}, 1E3)
				}
			}
		},
		R = za("script"),
		fe = Xa(215, function(a) {
			var b = a.google_sa_queue,
				c = b.shift();
			a.google_sa_impl || dd("shimpl", {
				t: "no_fn"
			});
			"function" == ba(c) && na(216, Ka, c);
			b.length && a.setTimeout(function() {
				return fe(a)
			}, 0)
		}),
		ve = !1,
		Na = 0,
		we = !1,
		Be = !1,
		Oa = !1;
	Ce()
}).call(this);