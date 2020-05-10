﻿// The FinderOuter
// Copyright (c) 2020 Coding Enthusiast
// Distributed under the MIT software license, see the accompanying
// file LICENCE or http://www.opensource.org/licenses/mit-license.php.

using FinderOuter.Services;
using Xunit;

namespace Tests.Services
{
    public class InputServiceTests
    {
        private const string ValidCompKey = "5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJ";
        private const string ValidUnCompKey1 = "KwdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP98617";
        private const string ValidUnCompKey2 = "L53fCHmQhbNp1B4JipfBtfeHZH7cAibzG9oK19XfiFzxHgAkz6JK";

        private const string ValidP2pkhAddr = "1BvBMSEYstWetqTFn5Au4m4GFg7xJaNVN2";
        private const string ValidP2shAddr = "3J98t1WpEZ73CNmQviecrnyiWrnqRhWNLy";


        [Theory]
        [InlineData("6PRWdmoT1ZursVcr5NiD14p5bHrKVGPG7yeEoEeRb8FVaqYSHnZTLEbYsU", "The given BIP-38 string is valid.")]
        [InlineData("6PnZki3vKspApf2zym6Anp2jd5hiZbuaZArPfa2ePcgVf196PLGrQNyVUh", "The given BIP-38 string is valid.")]
        [InlineData("6PRWdmoT1ZursVcr5NiD14p5bHrKVGPG7yeEoEeRb8FVaqYSHnZTLEbYs$", "The given BIP-38 string contains invalid base-58 characters.")]
        [InlineData("6PRWdmoT1ZursVcr5NiD14p5bHrKVGPG7yeEoEeRb8FVaqYSHnZTLEbYs1", "The given BIP-38 string has an invalid checksum.")]
        [InlineData("2DnRqfF9cUPrMxRSAbprPfviNN37TLoH7Zmgq5uS4CcTQymH9nfcFXvXX", "The given BIP-38 string has an invalid byte length.")]
        [InlineData("AfEEGJ8HqcGUofEyL7Cr6R73LJbus3tuKFMHEiBmT6X1H8npuj94cMrcai", "The given BIP-38 string has invalid starting bytes.")]
        [InlineData("6RMoGm8dMt4BH2WLE6jLYNeF6B4SZ4WHmg6PRggwCQYqJPPwU32uVBH8Be", "The given BIP-38 string has invalid starting bytes.")]
        public void CheckBase58Bip38Test(string bip38, string expectedMsg)
        {
            InputService serv = new InputService();
            string actualMsg = serv.CheckBase58Bip38(bip38);
            Assert.Equal(expectedMsg, actualMsg);
        }


        [Theory]
        [InlineData(ValidP2pkhAddr, "The given address is a valid base-58 encoded address used for P2PKH scripts.")]
        [InlineData(ValidP2shAddr, "The given address is a valid base-58 encoded address used for P2SH scripts.")]
        [InlineData("1BvBMSEYstWetqTFn5Au4m4$Fg7xJaNVN2", "The given address contains invalid base-58 characters.")]
        [InlineData("1BvBMSEYstWetqTFn5Au4m4GFg7xJaNVN3", "The given address has an invalid checksum.")]
        [InlineData("12eESoee9vDq6tQtZ6RQfdf3SsHWBQYpd", "The given address byte length is invalid.")]
        [InlineData("34q4KRuJeVGJ79f8jRkexoEnFKP1fRjqp", "The given address starts with an invalid byte.")]
        public void CheckBase58AddressTest(string addr, string expectedMsg)
        {
            InputService serv = new InputService();
            string actualMsg = serv.CheckBase58Address(addr);
            Assert.Equal(expectedMsg, actualMsg);
        }


        [Theory]
        [InlineData(ValidCompKey)]
        [InlineData(ValidUnCompKey1)]
        [InlineData(ValidUnCompKey2)]
        [InlineData("5HueCGU8*MjxEXxiPuD5BDku4MkFqeZyd4dZ*jvhTVqvbTLvyT*")]
        [InlineData("K%dMAjG^erYan$eui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP9*617")]
        [InlineData("L53fCHmQhbNp1B4JipfBtf*HZH7cAibzG9oK19X(iFzxHgAkz6JK")]
        public void CanBePrivateKeyTest(string key)
        {
            InputService serv = new InputService();
            bool actual = serv.CanBePrivateKey(key, out string error);
            Assert.True(actual, error);
            Assert.Null(error);
        }

        [Theory]
        [InlineData("")]
        [InlineData("5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyT")]
        [InlineData("5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyT1234")]
        [InlineData("5HueCGU8*MjxEXxiPuD5BDku4MkFqeZyd4dZ*jvhTVqvbTLvy*")]
        [InlineData("KwdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP9867")]
        [InlineData("Kw")]
        [InlineData("KwdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP98671234")]
        [InlineData("L53fCHmQhbNp1B4JipfBtfeHZH7cAibzG9oK19XfiFzxHgAkz6J")]
        [InlineData("L53fCHmQhbNp1B4JipfBtfeHZH7cAibzG9oK19XfiFzxHgAkz6JK1")]
        public void CanBePrivateKey_FalseTest(string key)
        {
            InputService serv = new InputService();
            bool actual = serv.CanBePrivateKey(key, out _);
            Assert.False(actual);
        }

        [Theory]
        [InlineData(ValidCompKey, '*')]
        [InlineData("5HueCGU8r*jxEXxi*uD5*Dku4MkFqeZyd4dZ1jvhTVqvbTL*yTJ", '*')]
        [InlineData("5HueCG--------------------kFqeZyd4dZ1jvhTVqvbTLvyTJ", '-')]
        [InlineData(ValidUnCompKey1, '*')]
        [InlineData("KwdMAjGmerYanjeui5SHS7J*mpZvVipYvB2LJGU1ZxJwYvP98617", '*')]
        [InlineData(ValidUnCompKey2, '*')]
        [InlineData("L53fCHmQ$$Np1B4JipfBt$eHZH$cAibz$9oK1$XfiFzxHgAkz6$$", '$')]

        [InlineData("5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLv", '$')]
        [InlineData("5HueGU8rMjxExPuD5BDku4kFqeZyddZjvhTVvbLvyTJ", '$')]
        [InlineData("KwdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP9", '$')]
        [InlineData("L53fCHmNp1B4JipfBtfeHZH7cAibzG9oK19XfiFzxHgA", '$')]
        public void CheckIncompletePrivateKeyTest(string key, char missingChar)
        {
            InputService serv = new InputService();
            bool actual = serv.CheckIncompletePrivateKey(key, missingChar, out string error);
            Assert.True(actual, error);
            Assert.Null(error);
        }

        [Theory]
        [InlineData(ValidCompKey, '`', "Invalid missing character. Choose one from")]
        [InlineData(null, '*', "Key can not be null or empty.")]
        [InlineData(" ", '*', "Key can not be null or empty.")]
        [InlineData("5HueCGU8rMjxEXxiPuD5BDk$4MkFqeZyd4dZ1jvhTVqvbTLvyTJ", '*', "Key contains invalid base-58 characters (ignoring the missing char = *).")]
        [InlineData("5HueCGU8rMjxEXx*PuD5BDk$4MkFqeZyd4dZ1jvhTVqvbTLvyTJ", '*', "Key contains invalid base-58 characters (ignoring the missing char = *).")]
        [InlineData("5HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJO", '*', "Key contains invalid base-58 characters (ignoring the missing char = *).")]
        [InlineData("5HueCGU8*MjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvy", '*', "Invalid key length.")]
        [InlineData("KwdMAjGmerYanjeui**HS7JkmpZvVi*YvB2LGU1*xJwYv*8617", '*', "Invalid key length.")]
        [InlineData("L53f*HHmQhbNp1B4JipfBtfeHZH7cAibzG9oK19XfiFzxHgAkz6JK1", '*', "Invalid key length.")]
        [InlineData("6HueCGU8rMjxEXxiPuD5BDk*4MkFqeZyd4dZ1jvhTVqvbTLvyTJ", '*', "Invalid first character for an uncompressed private key considering length.")]
        [InlineData("LHueCGU8rMjxEXxiPu*5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJ", '*', "Invalid first character for an uncompressed private key considering length.")]
        [InlineData("KHueCGU8rMjxEXxiPuD5BDku4MkFqeZy*4dZ1jvhTVqvbTLvyTJ", '*', "Invalid first character for an uncompressed private key considering length.")]
        [InlineData("XwdMAjGmerYanjeui5SHS*JkmpZvVipYvB2LJGU1ZxJwYvP98617", '*', "Invalid first character for a compressed private key considering length.")]
        [InlineData("5wdMAjGmerYanjeui5SH*7JkmpZvVipYvB2LJGU1ZxJwYvP98617", '*', "Invalid first character for a compressed private key considering length.")]

        [InlineData("KwdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP986171", '*', "Key length is too big.")]
        [InlineData("5wdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP98617", '*', "Invalid first key character considering its length.")]
        [InlineData("UwdMAjGmerYanjeui5SHS7JkmpZvVipYvB2LJGU1ZxJwYvP98617", '*', "Invalid first key character considering its length.")]
        [InlineData("KHueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJ", '*', "Invalid first key character considering its length.")]
        [InlineData("6HueCGU8rMjxEXxiPuD5BDku4MkFqeZyd4dZ1jvhTVqvbTLvyTJ", '*', "Invalid first key character considering its length.")]
        [InlineData("mwdMAjGmerYanjeui5SHS7Jkm", '*', "The first character of the given private key is not valid.")]
        public void CheckIncompletePrivateKey_FailTest(string key, char missingChar, string expError)
        {
            InputService serv = new InputService();
            bool actual = serv.CheckIncompletePrivateKey(key, missingChar, out string error);
            Assert.False(actual);
            Assert.Contains(expError, error);
        }

        [Theory]
        [InlineData('*', true)]
        [InlineData('-', true)]
        [InlineData('$', true)]
        [InlineData('_', true)]
        [InlineData(' ', false)]
        [InlineData('a', false)]
        [InlineData('B', false)]
        [InlineData('`', false)]
        [InlineData('(', false)]
        public void IsMissingCharValidTest(char c, bool expected)
        {
            InputService serv = new InputService();
            Assert.Equal(expected, serv.IsMissingCharValid(c));
        }

        [Theory]
        [InlineData("1BvBMSEYstWetqTFn5Au4m4GFg7xJaNVN2", true, "77bff20c60e522dfaa3350c39b030a5d004e839a")]
        [InlineData("bc1qar0srrr7xfkvy5l643lydnw9re59gtzzwf5mdq", true, "e8df018c7e326cc253faac7e46cdc51e68542c42")]
        public void IsValidAddressTest(string addr, bool ignore, string expectedHash)
        {
            InputService serv = new InputService();
            bool actual = serv.IsValidAddress(addr, ignore, out byte[] actualHash);
            Assert.True(actual);
            Assert.Equal(Helper.HexToBytes(expectedHash), actualHash);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("1BvBMSEYstWetqTFn5Au4m4GFg7xJaNVN1", true)] // Invalid checksum
        [InlineData("1#vBMSEYstWetqTFn5Au4m4GFg7xJaNVN2", true)] // Invalid char
        [InlineData("3J98t1WpEZ73CNmQviecrnyiWrnqRhWNLy", true)] // Valid P2SH address
        [InlineData("3J98t1WpEZ73CNmQviecrnyiWrnqRhWNL1", false)] // Invalid P2SH address (checksum)
        [InlineData("tb1qar0srrr7xfkvy5l643lydnw9re59gtzzwf5mdq", true)]
        [InlineData("bc1qar0srrr7xfkvy5l643lydnw9re59gtzzwf5md2", true)]
        public void IsValidAddress_FalseTest(string addr, bool ignore)
        {
            InputService serv = new InputService();
            bool actual = serv.IsValidAddress(addr, ignore, out byte[] actualHash);
            Assert.False(actual);
            Assert.Null(actualHash);
        }
    }
}
