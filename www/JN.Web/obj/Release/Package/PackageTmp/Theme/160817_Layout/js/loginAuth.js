/**
 * loginAuth: login form auth
 * @author Jeff
 * @param  Object w        各站字典檔
 * @param  Boolean chkOpen 驗證碼開關
 * @return Null
 */
(function($) {
    $.fn.loginAuth = function(w, chkOpen) {
        "use strict";

        // 只有一個引數時，為設定驗證碼開關
        if(typeof w === 'boolean' && typeof chkOpen === 'undefined') {
            chkOpen = w;
        }
        chkOpen = (typeof chkOpen !== 'undefined' && typeof chkOpen === 'boolean')? chkOpen: true;

        var self = $(this),
            // 帳號、密碼、驗證碼資訊
            inputInfo = {
                username   : $("input[name=username]", self), // 帳號欄位
                passwd     : $("input[name=passwd]", self),   // 密碼欄位
                chkCode    : $("input[name=rmNum]", self),    // 驗證碼欄位
                usernameMin: 4,  // 帳號最小長度
                usernameMax: 15, // 帳號最大長度
                passwdMin  : 6,  // 密碼最小長度
                passwdMax  : 12  // 密碼最大長度
            },
            // 驗證表達式
            reg = {
                acc          : /[a-zA-Z0-9]/g,  // 帳號規則
                accReverse   : /[^a-zA-Z0-9]/g, // 帳號規則(反)
                passwd       : /[a-zA-Z0-9]/g,     // 密碼規則
                passwdReverse: /[^a-zA-Z0-9]/g,    // 密碼規則(反)
                upper        : /[A-Z]/g         // 大寫鎖定
            },
            // 字典檔
            dictionary = {
                'accNull'    : '请输入帐号！',
                'accShort'   : '帐号长度不能少于%s个字元',
                'accLong'    : '帐号长度不能多于%s个字元',
                'accFalse'   : '帐号须符合0~9、a~z及A~Z字元',
                'pwNull'     : '请输入密码！',
                'pwShort'    : '密码长度不能少于%s个字元',
                'pwLong'     : '密码长度不能多于%s个字元',
                'pwFalse'    : '密码须符合0~9、a~z及A~Z字元',
                'pwUpper'    : '提醒：密码须为小写，大写锁定启用中',
                'chkCodeNull': '请输入验证码！'
            },
            // 事件處理
            authEvent = {
                accRealTime: function() {
                    // 輸入格式錯誤
                    if(reg.accReverse.test(this.value)) {
                        alert(dictionary.accFalse);
                        this.value = this.value.replace(reg.accReverse, '');
                    }

                    // 帳號過長
                    if(this.value.length > inputInfo.usernameMax) {
                        alert(dictionary.accLong);
                        this.value = this.value.substring(0, inputInfo.usernameMax);
                    }
                },
                pwRealTime: function() {
                    var isFalse = reg.passwdReverse.test(this.value);

                    // 格式錯誤
                    if(isFalse) {
                        alert(dictionary.pwFalse);
                        this.value = this.value.replace(reg.passwdReverse, '');
                    }

                    // 密碼過長
                    if(this.value.length > inputInfo.passwdMax) {
                        alert(dictionary.pwLong);
                        this.value = this.value.substring(0, inputInfo.passwdMax);
                    }
                },
                formSubmit: function (e) {
                    var userVal = inputInfo.username.val(),
                        pwVal = inputInfo.passwd.val(),
                        chkCodeVal = inputInfo.chkCode.val(),
                        isFalse = false;

                    // 帳號檢查
                    if(userVal === '') {
                        stopSend(dictionary.accNull, inputInfo.username);
                    } else if(userVal.length < inputInfo.usernameMin) {
                        stopSend(dictionary.accShort, inputInfo.username);
                    } else if(reg.accReverse.test(userVal)) {
                        stopSend(dictionary.accFalse, inputInfo.username);
                    }
                    if(isFalse) return;

                    // 密碼檢查
                    if(pwVal === '') {
                        stopSend(dictionary.pwNull, inputInfo.passwd);
                    } else if(pwVal.length < inputInfo.passwdMin) {
                        stopSend(dictionary.pwShort, inputInfo.passwd);
                    } else if(reg.passwdReverse.test(pwVal)) {
                        stopSend(dictionary.pwFalse, inputInfo.passwd);
                    }
                    if(isFalse) return;

                    // 驗證碼檢查
                    if(chkCodeVal === '' && chkOpen === true) {
                        stopSend(dictionary.chkCodeNull, inputInfo.chkCode);
                    }

                    /**
                     * 停止送出表單
                     * @param  String msg 彈跳訊息
                     * @param  Object ele focus 元素
                     * @return Null
                     */
                    function stopSend(msg, ele) {
                        if(!msg || !ele) {
                            e.preventDefault();
                            return;
                        }
                        isFalse = true;
                        alert(msg);
                        ele.focus();
                        e.preventDefault();
                    }
                }
            };

        // 使用者自訂字典檔
        if(typeof w === 'object') {
            dictionary = $.extend(dictionary, w);
        }

        // 字典檔取代
        dictionary.accShort = dictionary.accShort.replace("%s", inputInfo.usernameMin);
        dictionary.accLong  = dictionary.accLong.replace("%s", inputInfo.usernameMax);
        dictionary.pwShort  = dictionary.pwShort.replace("%s", inputInfo.passwdMin);
        dictionary.pwLong   = dictionary.pwLong.replace("%s", inputInfo.passwdMax);

        // 為配合即時驗證密碼長度，帳號、密碼輸入多一碼
        if(inputInfo.username.attr('maxlength') != inputInfo.usernameMax + 1) inputInfo.username.attr('maxlength', inputInfo.usernameMax + 1);
        if(inputInfo.passwd.attr('maxlength') != inputInfo.passwdMax + 1) inputInfo.passwd.attr('maxlength', inputInfo.passwdMax + 1);

        // 帳號即時驗證
        inputInfo.username.keyup(authEvent.accRealTime);

        // 密碼即時驗證
        inputInfo.passwd.keyup(authEvent.pwRealTime);

        // submit 驗證
        self.submit(authEvent.formSubmit);
    }
}(jQuery));