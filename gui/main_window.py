from constants import WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_TITLE, GRID_ROWS, GRID_COLS
from services.base_feedback_service import BaseFeedbackService
from services.base_guess_service import BaseGuessService
from models.main_window_state import MainWindowState
import tkinter.messagebox
import customtkinter
import tkinter

customtkinter.set_appearance_mode("System")  
customtkinter.set_default_color_theme("blue") 

class MainWindow(customtkinter.CTk):
    def __init__(self, state: MainWindowState, guess_service: BaseGuessService, feedback_service: BaseFeedbackService):
        super().__init__()
        
        self.__feedback_service = feedback_service
        self.__guess_service = guess_service
        self.__state = state
        
        self.__configure_window()   
        self.__configure_grid_layout()  
        self.__configure_left_frame()
        self.__configure_right_frame()

    def __configure_window(self):
        self.title(WINDOW_TITLE)
        self.geometry(f"{WINDOW_WIDTH}x{WINDOW_HEIGHT}")
        self.resizable(False, False)
        self.protocol("WM_DELETE_WINDOW", self.__on_closing) 
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)

    def __configure_left_frame(self): 
        self.frame_left = customtkinter.CTkFrame(master=self,width=180,corner_radius=0)
        self.frame_left.grid(row=0, column=0, sticky="nswe")

        self.frame_left.grid_rowconfigure(0, minsize=10)   # empty row with minsize as spacing
        self.frame_left.grid_rowconfigure(5, weight=1)  # empty row as spacing
        self.frame_left.grid_rowconfigure(8, minsize=20)    # empty row with minsize as spacing
        self.frame_left.grid_rowconfigure(11, minsize=10)  # empty row with minsize as spacing

        self.label_1 = customtkinter.CTkLabel(master=self.frame_left,text="Options:",text_font=("Roboto Medium", -16))  # font name and size in px
        self.label_1.grid(row=1, column=0, pady=10, padx=10)

        self.button_1 = customtkinter.CTkButton(master=self.frame_left,text="Reset Game",command=self.__reset)
        self.button_1.grid(row=2, column=0, pady=10, padx=20)

        self.button_2 = customtkinter.CTkButton(master=self.frame_left,text="Next Guess",command=self.__next_guess)
        self.button_2.grid(row=3, column=0, pady=10, padx=20)

        self.label_mode = customtkinter.CTkLabel(master=self.frame_left, text="Appearance Mode:")
        self.label_mode.grid(row=9, column=0, pady=0, padx=20, sticky="w")

        self.optionmenu_1 = customtkinter.CTkOptionMenu(master=self.frame_left,values=["Light", "Dark", "System"],command=self.__change_appearance_mode)
        self.optionmenu_1.grid(row=10, column=0, pady=10, padx=20, sticky="w")

    def __configure_right_frame(self): 
        self.frame_right = customtkinter.CTkFrame(master=self)
        self.frame_right.grid(row=0, column=1, sticky="nswe", padx=20, pady=20)

        self.frame_right.rowconfigure((0, 1, 2, 3), weight=1)
        self.frame_right.rowconfigure(7, weight=10)
        self.frame_right.columnconfigure((0, 1), weight=1)
        self.frame_right.columnconfigure(2, weight=0)

        self.frame_info = customtkinter.CTkFrame(master=self.frame_right)
        self.frame_info.grid(row=0, column=0, columnspan=2, rowspan=4, pady=20, padx=20, sticky="nsew")

        self.frame_info.rowconfigure(0, weight=1)
        self.frame_info.columnconfigure(0, weight=1)

        self.__update()

        self.entry = customtkinter.CTkEntry(master=self.frame_right,width=120,placeholder_text="Enter your guess here")
        self.entry.grid(row=8, column=0, columnspan=2, pady=20, padx=20, sticky="we")

        self.optionmenu_1.set("Dark") 

    def __configure_grid_layout(self):
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)
        self.frame_left = customtkinter.CTkFrame(master=self, width=180,corner_radius=0)
        self.frame_left.grid(row=0, column=0, sticky="nswe")
        self.frame_right = customtkinter.CTkFrame(master=self)
        self.frame_right.grid(row=0, column=1, sticky="nswe", padx=20, pady=20)

    def __reset(self) -> None:
        self.__state.reset()
        self.__update()

    def __next_guess(self):
        guess = self.__get_next_guess()
        pattern = self.__feedback_service.calculate(answer=self.__state.get_word_to_guess(), word=guess.guess_string)
       
        self.__state.add_pattern(pattern)
        self.__state.add_guess(guess)   
         
        self.__update()
    
    def __get_next_guess(self):
        if self.__state.get_current_iteration() == 0:
            return self.__guess_service.get_opener()
        return self.__guess_service.get_guess(self.__state.get_last_pattern(), self.__state.get_last_guess_id())

    def __change_appearance_mode(self, new_appearance_mode):
        customtkinter.set_appearance_mode(new_appearance_mode)

    def __on_closing(self):
        self.destroy()

    def __clear(self) -> None:
        for row in range(GRID_ROWS):
            for col in range(GRID_COLS):
                self.label_info_1 = customtkinter.CTkLabel(master=self.frame_info,text=" ",height=50,width=50,corner_radius=6,fg_color=("white", "gray38"),justify=tkinter.LEFT)
                self.label_info_1.grid(column=col, row=row, padx=15, pady=15)
                self.label_info_1.configure(fg_color=("grey", "gray38"))
                
    def __update(self) -> None:
        self.__clear()
        
        guesses = self.__state.get_guesses()
        for row in range(len(guesses)):
            for col in range(GRID_COLS):
                value = guesses[row].guess_string[col]
                self.label_info_1 = customtkinter.CTkLabel(master=self.frame_info,text=" ",height=50,width=50,corner_radius=6,fg_color=("white", "gray38"),justify=tkinter.LEFT)
                self.label_info_1.grid(column=col, row=row, padx=15, pady=15)
                self.label_info_1.configure(text=value)
            
        patterns = self.__state.get_patterns()
        for row in range(len(patterns)):
            for col in range(GRID_COLS):
                value = patterns[row][col]
                self.label_info_1 = customtkinter.CTkLabel(master=self.frame_info,text=" ",height=50,width=50,corner_radius=6,fg_color=("white", "gray38"),justify=tkinter.LEFT)
                self.label_info_1.grid(column=col, row=row, padx=15, pady=15)
                self.label_info_1.configure(text=guesses[row].guess_string[col])
                self.label_info_1.configure(fg_color=self.__get_label_color(value))
                
    def __get_label_color(self, value: str) -> tuple: 
        if value == "0":
            return ("white", "gray38")
        if value == "1":
            return ("yellow", "gray38")
        if value == "2":
           return ("green", "gray38")
        return ("grey", "gray38")