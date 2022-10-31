using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZedCrestWalletApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // the accountservice is injected here
        private IAccountService _accountService;
        // Add Automapper
        IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        // GET: api/<AccountController>
        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _accountService.GetAllAccount();
            var clearedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(clearedAccounts);
        }
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            return Ok(_accountService.Authenticate(model.AccountNumber,model.Pin));
            //it returns an account..let's seewhen we run before
        }
        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if(!Regex.IsMatch(AccountNumber, @"^[0-9]{10}$|^[0-9]{11}$")) return BadRequest("Account number must be 10-digit");
            var account = _accountService.GetByAccountNumber(AccountNumber);
            var clearedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(clearedAccount);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int Id)
        {
            var account = _accountService.GetById(Id);
            var clearedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(clearedAccount);
        }

        [HttpPost]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var account = _mapper.Map<CustomerAccountInformation>(model);
            _accountService.Update(account, model.Pin);
            return Ok();
        }

        // POST api/<AccountController>
        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterAccountModel newAccount)
        {
            if (!ModelState.IsValid) return BadRequest(newAccount);
            //map to account
            var account = _mapper.Map<CustomerAccountInformation>(newAccount);
            return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin));
        }

    }
}
