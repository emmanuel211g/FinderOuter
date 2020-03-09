﻿// The FinderOuter
// Copyright (c) 2020 Coding Enthusiast
// Distributed under the MIT software license, see the accompanying
// file LICENCE or http://www.opensource.org/licenses/mit-license.php.

using FinderOuter.Backend;
using FinderOuter.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinderOuter.ViewModels
{
    public class MissingBase58ViewModel : OptionVmBase
    {
        public MissingBase58ViewModel()
        {
            // Don't move this line, service must be instantiated here
            InputService inServ = new InputService();
            b58Service = new Base58Sevice(Result);

            IObservable<bool> isFindEnabled = this.WhenAnyValue(
                x => x.Input, x => x.MissingChar,
                x => x.Result.CurrentState, (b58, c, state) =>
                            !string.IsNullOrEmpty(b58) &&
                            inServ.IsMissingCharValid(c) &&
                            state != Models.State.Working);

            FindCommand = ReactiveCommand.Create(Find, isFindEnabled);
            InputTypeList = Enum.GetValues(typeof(Base58Sevice.InputType)).Cast<Base58Sevice.InputType>();
        }



        public override string OptionName => "Missing Base58";
        public override string Description => $"Helps you recover missing base-58 characters in any base-58 encoded strings that " +
            $"has a checksum. Examples are private keys, extended pub/priv keys, addresses,...{Environment.NewLine}" +
            $"Enter the base-58 string and replace its missing characters with the symbol defined by {nameof(MissingChar)} " +
            $"parameter and press Find.";


        private readonly Base58Sevice b58Service;

        public IEnumerable<Base58Sevice.InputType> InputTypeList { get; private set; }

        private Base58Sevice.InputType _selInpT;
        public Base58Sevice.InputType SelectedInputType
        {
            get => _selInpT;
            set => this.RaiseAndSetIfChanged(ref _selInpT, value);
        }

        private string _input;
        public string Input
        {
            get => _input;
            set => this.RaiseAndSetIfChanged(ref _input, value);
        }

        private char _mis = '*';
        public char MissingChar
        {
            get => _mis;
            set => this.RaiseAndSetIfChanged(ref _mis, value);
        }

        public string MissingToolTip => $"Choose one of these symbols {Constants.Symbols} to use instead of the missing characters";

        public override void Find()
        {
            _ = b58Service.Find(Input, MissingChar, SelectedInputType);
        }
    }
}
